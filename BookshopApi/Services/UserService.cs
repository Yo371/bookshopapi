using System.Buffers.Text;
using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using BookshopApi.Utils;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();

    Task<User> GetUser(int id);

    Task<User> GetUser(int id, string role, string loggedId);

    Task CreateUser(User user);

    Task UpdateUser(User user);

    Task<bool> UpdateUser(User user, string role, string loggedId);

    Task DeleteUser(int id);

    Task<bool> DeleteUser(int id, string role, string loggedId);

    Task<ValidationModel> GetValidatedUser(string login, string password);
}

public class UserService : IUserService
{
    private readonly BookShopContext _context;

    private const string CustomerRole = "Customer";

    public UserService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        try
        {
            return await _context.Users.Include(u => u.Auth)
                .Select(u => u.Adapt<User>()).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all users.", ex);
        }
    }

    public async Task<User> GetUser(int id)
    {
        try
        {
            return (await _context.Users.Include(u => u.Auth).FirstOrDefaultAsync(e => e.Id == id))
                .Adapt<User>();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the user with ID {id}.", ex);
        }
    }

    public async Task<User> GetUser(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
                return await GetUser(id);
            else
                return null;
        }

        return await GetUser(id);
    }

    public async Task CreateUser(User user)
    {
        try
        {
            var userEntity = user.Adapt<UserEntity>();

            var isLoginExisted = _context.Users.Any(e => e.Login.Equals(userEntity.Login));

            if (isLoginExisted)
            {
                throw new InvalidOperationException("Login already exists");
            }

            var role = await _context.AuthModels.FirstOrDefaultAsync(e => e.Role == userEntity.Auth.Role);
            userEntity.Auth = role;

            userEntity.Password = PasswordHelper.HashPasword(user.Password, out var salt);
            userEntity.Salt = salt;

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();
            user.Id = userEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating user.", ex);
        }
    }

    public async Task UpdateUser(User user)
    {
        try
        {
            var userEntity = user.Adapt<UserEntity>();
            var role = await _context.AuthModels.FirstOrDefaultAsync(e => e.Role == userEntity.Auth.Role);
            userEntity.Auth = role;
            _context.Users.Update(userEntity);
            _context.Entry(userEntity).Property(x => x.Salt).IsModified = false;
            _context.Entry(userEntity).Property(x => x.Password).IsModified = false;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating user.", ex);
        }
    }

    public async Task<bool> UpdateUser(User user, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(user.Id.ToString()))
            {
                await UpdateUser(user);
                return true;
            }
            else
                return false;
        }

        await UpdateUser(user);
        return true;
    }

    public async Task DeleteUser(int id)
    {
        try
        {
            _context.Users.Remove(_context.Users.Find(id));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the user with ID {id}.", ex);
        }
    }

    public async Task<bool> DeleteUser(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
            {
                await DeleteUser(id);
                return true;
            }
            else
                return false;
        }

        await DeleteUser(id);
        return true;
    }

    public async Task<ValidationModel> GetValidatedUser(string login, string password)
    {
        try
        {
            ValidationModel userValidModel;

            if (_context.Users.Any(e => e.Login.Equals(login)))
            {
                var salt = (await _context.Users.FirstOrDefaultAsync(e => e.Login.Equals(login))).Salt;
                var hashPasswordFromDb =
                    (await _context.Users.FirstOrDefaultAsync(e => e.Login.Equals(login))).Password;

                var isPasswordMathed = PasswordHelper.VerifyPassword(password, hashPasswordFromDb, salt);

                if (isPasswordMathed)
                {
                    var userFromDb = await _context.Users
                        .Include(u => u.Auth).FirstOrDefaultAsync(u => u.Login.Equals(login));
                    userValidModel = new ValidationModel()
                    {
                        IsCredentialsMatched = true,
                        Role = userFromDb.Auth.Role,
                        Id = userFromDb.Id
                    };
                }
                else
                {
                    userValidModel = new ValidationModel()
                    {
                        IsCredentialsMatched = false,
                    };
                }
            }
            else
            {
                userValidModel = new ValidationModel()
                {
                    IsCredentialsMatched = false,
                };
            }

            return userValidModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while validating authorized user.", ex);
        }
    }
}