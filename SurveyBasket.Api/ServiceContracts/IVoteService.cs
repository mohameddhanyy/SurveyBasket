using SurveyBasket.Api.DTOs.Vote;

namespace SurveyBasket.Api.ServiceContracts
{
    public interface IVoteService
    {
        public Task<Result> AddAsync(int pollId, string userId, VoteRequest request);
    }
}
