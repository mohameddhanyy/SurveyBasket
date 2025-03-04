namespace SurveyBasket.Api.DTOs.Auth
{
    public record AuthResponse(
        string Id,
        string? Email,
        string FisrtName,
        string lastName,
        string Token,
        int ExpiresIn,
        string RefreshToken,
        DateTime RefreshTokenExpiration
    );
}
