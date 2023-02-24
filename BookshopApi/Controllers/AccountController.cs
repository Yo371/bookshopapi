using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookshopApi.Services;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookshopApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    
    private readonly IAuthService _authService;
    
    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost()]
    [Route("api/token")]
    public async Task<IActionResult> Token([FromBody] UserLogin userLogin)
    {
        var identity = await GetIdentity(userLogin.Username, userLogin.Password);
        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }
 
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
        var response = new
        {
            access_token = encodedJwt,
        };
 
        return Json(response);
    }
 
    private async Task<ClaimsIdentity> GetIdentity(string username, string password)
    {
        var person = await _authService.GetValidatedUserAsync(username, password);

        if (person.IsCredentialsMatched)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, person.Role.ToString()),
                new Claim(ClaimTypes.SerialNumber, person.Id.ToString())
            };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimTypes.Name,
                    ClaimTypes.Role);
            return claimsIdentity;
        }
        
        return null;
    }
}