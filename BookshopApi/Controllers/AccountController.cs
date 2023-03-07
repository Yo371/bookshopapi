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
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthService authService, ILogger<AccountController> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        //Thrown exception in case of null
        _logger = logger;
    }
    
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/token")]
    public async Task<IActionResult> Token([FromBody] UserLogin userLogin)
    {
        try
        {
            var identity = await GetIdentity(userLogin.Username, userLogin.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.TokenExpirationMinutes)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
            };

            return Json(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't generate token.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
 
    private async Task<ClaimsIdentity> GetIdentity(string username, string password)
    {
        var person = await _authService.GetValidatedUserAsync(username, password);

        if (person?.IsCredentialsMatched ?? false)
        //Add a null check to the "person"  to ensure that the code doesn't throw exception
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