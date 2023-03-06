using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookshopApi;

public class AuthOptions
{
    public static string Issuer { get; set; } = "https://localhost:7115/";
    public static string Audience { get; set; } = "https://localhost:7115/";
    const string KEY = "NxIKgKNt74P7T3CvoJTd";
    public static int TokenExpirationMinutes { get; set; } = 15;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}