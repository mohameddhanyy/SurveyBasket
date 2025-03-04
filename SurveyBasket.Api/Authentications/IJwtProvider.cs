using SurveyBasket.Api.Presistance.Models;

namespace SurveyBasket.Api.Authentications
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user);

        string? ValidateToken(string token);
    }
}
