using BookshopApi.DataAccess;
using BookshopApi.Utils;
using Commons.Models;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IAuthService
{
    Task<ValidationModel> GetValidatedUserAsync(string login, string password);
}

public class AuthService : IAuthService
{
    private readonly BookShopContext _context;

    public AuthService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<ValidationModel> GetValidatedUserAsync(string login, string password)
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