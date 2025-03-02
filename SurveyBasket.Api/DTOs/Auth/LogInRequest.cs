namespace SurveyBasket.Api.DTOs.Auth
{
    public record LogInRequest(
        string Email,
        string Password
        );
}
