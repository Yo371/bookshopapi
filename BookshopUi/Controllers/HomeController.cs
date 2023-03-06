using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Commons.Models;
using Commons.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;
using System.Configuration;

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
                var token =  _authApiService.Authenticate(userLogin);

                if (token == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return RedirectToAction(nameof(InvalidLoginAttempt));
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
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    RedirectUri = "/home/index"
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);


                return RedirectToAction(nameof(Greeting));
            }

            return View(userLogin);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
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