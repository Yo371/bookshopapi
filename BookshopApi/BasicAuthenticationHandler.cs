using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BookshopApi.Services;
using Commons.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BookshopApi;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthService _authService;
    
    public BasicAuthenticationHandler(
        IAuthService authService,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder, 
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string username = null;
        ValidationModel userValidModel = null;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            username = credentials.FirstOrDefault();
            var password = credentials.LastOrDefault();
            userValidModel = await _authService.GetValidatedUserAsync(username, password);

            if (!userValidModel.IsCredentialsMatched)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, userValidModel.Role.ToString()),
                new Claim(ClaimTypes.SerialNumber, userValidModel.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (FormatException ex)
        {
            return AuthenticateResult.Fail("Invalid authorization header format");
        }
        catch (InvalidOperationException ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }
}