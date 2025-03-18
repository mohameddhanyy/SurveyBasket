namespace SurveyBasket.Api.DTOs.Results
{
    public record VotesPerDayResponse(
        int NOVotes,
        DateOnly Date
);
}
