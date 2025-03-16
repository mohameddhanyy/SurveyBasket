namespace SurveyBasket.Api.DTOs.Vote
{
    public record VoteRequest(
        IEnumerable<VoteResponse> Answers
);
}
