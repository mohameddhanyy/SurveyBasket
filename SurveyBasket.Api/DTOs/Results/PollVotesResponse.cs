namespace SurveyBasket.Api.DTOs.Results
{
    public record PollVotesResponse(
        string Titel , 
        IEnumerable<VoteResponse> Votes
);
}
