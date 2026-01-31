namespace SurveyBasket.Api.DTOs.Auth
{
    public record ConfirmEmailRequest(
        string UserId,
        string Code
        );
}
