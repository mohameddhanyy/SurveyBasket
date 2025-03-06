namespace SurveyBasket.Api.DTOs.Polls
{
    public record PollResponse(
        string Title,
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt);
}
