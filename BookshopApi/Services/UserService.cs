using System.Buffers.Text;
using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Utils;
using Commons.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<User> GetUserAsync(int id);

    Task<User> GetUserAsync(int id, string role, string loggedId);

    Task CreateUserAsync(User user);

    Task UpdateUserAsync(User user);

    Task<bool> UpdateUserAsync(User user, string role, string loggedId);

    Task DeleteUserAsync(int id);

    Task<bool> DeleteUserAsync(int id, string role, string loggedId);
}

public class UserService : IUserService
{
    private readonly BookShopContext _context;

    private const string CustomerRole = "Customer";

    public UserService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            return (await _context.Users.Include(u => u.Auth).ToListAsync()).Adapt<IEnumerable<User>>();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all users.", ex);
        }
    }

    public async Task<User> GetUserAsync(int id)
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

    public async Task<User> GetUserAsync(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
                return await GetUserAsync(id);
            else
                return null;
        }

        return await GetUserAsync(id);
    }

    public async Task CreateUserAsync(User user)
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

    public async Task UpdateUserAsync(User user)
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

    public async Task<bool> UpdateUserAsync(User user, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(user.Id.ToString()))
            {
                await UpdateUserAsync(user);
                return true;
            }
            else
                return false;
        }

        await UpdateUserAsync(user);
        return true;
    }

    public async Task DeleteUserAsync(int id)
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

    public async Task<bool> DeleteUserAsync(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
            {
                await DeleteUserAsync(id);
                return true;
            }
            else
                return false;
        }

        await DeleteUserAsync(id);
        return true;
    }
}