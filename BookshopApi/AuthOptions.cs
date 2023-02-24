using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookshopApi;

public class AuthOptions
{
    public const string ISSUER = "https://localhost:7115/"; 
    public const string AUDIENCE = "https://localhost:7115/"; 
    const string KEY = "NxIKgKNt74P7T3CvoJTd";  
    public const int LIFETIME = 15; 
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}