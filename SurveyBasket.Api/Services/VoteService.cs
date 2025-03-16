using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.DTOs.Vote;
using SurveyBasket.Api.Presistance.Models;
using System.Threading;

namespace SurveyBasket.Api.Services
{
    public class VoteService(SurveyBasketDBContext context): IVoteService
    {
        private readonly SurveyBasketDBContext _context = context;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request)
        {
            var hasVote = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId);
            if (hasVote)
                return Result.Failure(VoteError.DuplicatedVote);

            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow) && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow));
            if (!pollIsExists)
                return Result.Failure(PollErrors.NoPollFound);

            var availabelQuestions = await _context.Questions
                .Where(x => x.PollId == pollId && x.IsActive)
                .Select(x => x.Id)
                .ToListAsync();
            if(!request.Answers.Select(x=>x.QuestionId).SequenceEqual(availabelQuestions))
                return Result.Failure(VoteError.InCorrectQuestions);

            var vote = new Vote()
            {
                PollId = pollId,
                UserId = userId,
                VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };

            await _context.AddAsync(vote);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
