using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User GetUser(int id);

    User GetUser(int id, string role, string loggedId);

    void CreateUser(User user);

    void UpdateUser(User user);
    
    bool UpdateUser(User user, string role, string loggedId);

    void DeleteUser(int id);
    
    bool DeleteUser(int id, string role, string loggedId);

    ValidationModel GetValidatedUser(string userName, string password);
}

public class UserService : IUserService
{
    private readonly BookShopContext _context;

    private const string CustomerRole = "Customer";

    public UserService(BookShopContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAllUsers()
    {
        var users = _context.Users.Include(u => u.Auth).Select(u => u.Adapt<User>()).ToList();

        return users;
    }

    public User GetUser(int id)
    {
        return _context.Users.Include(u => u.Auth).First(e => e.Id == id).Adapt<User>();
    }

    public User GetUser(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
                return GetUser(id);
            else
                return null;
        }

        return GetUser(id);
    }

    public void CreateUser(User user)
    {
        var userEntity = user.Adapt<UserEntity>();
        var role = _context.AuthModels.First(e => e.Role == userEntity.Auth.Role);
        userEntity.Auth = role;
        _context.Users.Add(userEntity);
        _context.SaveChanges();
        user.Id = userEntity.Id;
    }

    public void UpdateUser(User user)
    {
        var userEntity = user.Adapt<UserEntity>();
        var role = _context.AuthModels.First(e => e.Role == userEntity.Auth.Role);
        userEntity.Auth = role;
        _context.Users.Update(userEntity);
        _context.SaveChanges();
    }

    public bool UpdateUser(User user, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(user.Id.ToString()))
            {
                UpdateUser(user);
                return true;
            }
            else
                return false;
        }

        UpdateUser(user);
        return true;
    }

    public void DeleteUser(int id)
    {
        _context.Users.Remove(_context.Users.Find(id));
        _context.SaveChanges();
    }

    public bool DeleteUser(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
            {
                DeleteUser(id);
                return true;
            }
            else
                return false;
        }

        DeleteUser(id);
        return true;
    }

    public ValidationModel GetValidatedUser(string userName, string password)
    {
        var userFromDb = _context.Users.Include(u => u.Auth)
            .Where(u => u.Password.Equals(password) && u.Login.Equals(userName)).ToList();

        var userValidModel = new ValidationModel()
        {
            IsCredentialsMatched = userFromDb.Any(),
        };
        if (userValidModel.IsCredentialsMatched)
        {
            userValidModel.Role = userFromDb.First().Auth.Role;
            userValidModel.Id = userFromDb.First().Id;
        }

        return userValidModel;
    }
}