namespace SurveyBasket.Api.DTOs.Auth
{
    public record RefreshTokenRequest(
        string Token,
        string RefreshToken
        );
}
