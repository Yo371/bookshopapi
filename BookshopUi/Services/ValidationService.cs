using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Commons.Models;

namespace BookshopUi.Services;

public interface IValidationService
{
    ValidationModel GetValidationModel(HttpContext httpContext);
}

public class ValidationService : IValidationService
{
    public ValidationModel GetValidationModel(HttpContext httpContext)
    {
        ValidationModel validationModel;
        if (httpContext.User.Identity.IsAuthenticated)
        {
            var token = httpContext.User.Claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Authentication))?.Value;

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            validationModel = new ValidationModel()
            {
                IsCredentialsMatched = true,
                Role = Enum.Parse<Role>(jwtSecurityToken.Claims.First(e => e.Type.Equals(ClaimTypes.Role)).Value),
                Id = int.Parse(jwtSecurityToken.Claims.First(e => e.Type.Equals(ClaimTypes.SerialNumber)).Value),
                Token = token
            };
            return validationModel;
        }
        validationModel = new ValidationModel()
        {
            IsCredentialsMatched = false,
        };

        return validationModel;
    }
}