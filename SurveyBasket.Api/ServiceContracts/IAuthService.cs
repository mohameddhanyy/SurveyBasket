using SurveyBasket.Api.DTOs.Auth;

namespace SurveyBasket.Api.ServiceContracts
{
    public interface IAuthService
    {
        Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    }
}
