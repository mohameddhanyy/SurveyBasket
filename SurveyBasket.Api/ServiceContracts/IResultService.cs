using SurveyBasket.Api.DTOs.Results;

namespace SurveyBasket.Api.ServiceContracts
{
    public interface IResultService
    {
        Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId);
        Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId);
    }
}
