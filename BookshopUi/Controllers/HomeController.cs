using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Commons.Models;
using Commons.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;

namespace BookshopUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public HomeController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
            _authApiService.SetClient(new RestClient(Constants.ApiUrl));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var token = _authApiService.Authenticate(userLogin);

                if (token == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return RedirectToAction("InvalidLoginAttempt");
                }

                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var role = jwtToken.Claims.First(e => e.Type.Equals(ClaimTypes.Role)).Value;
                var id = jwtToken.Claims.First(e => e.Type.Equals(ClaimTypes.SerialNumber)).Value;
                var name = jwtToken.Claims.First(e => e.Type.Equals(ClaimTypes.Name)).Value;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Authentication, token),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.SerialNumber, id),
                    new Claim(ClaimTypes.Name, name)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);


                return RedirectToAction("Greeting");
            }

            return View(userLogin);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Greeting()
        {
            return View();
        }
        
        public IActionResult InvalidLoginAttempt()
        {
            return View();
        }
    }
}