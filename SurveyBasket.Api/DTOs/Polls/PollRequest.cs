namespace SurveyBasket.Api.DTOs.Polls
{
    public record PollRequest(
        string Title,
        string Summary,
        DateOnly StartsAt,
        DateOnly EndsAt);
}
