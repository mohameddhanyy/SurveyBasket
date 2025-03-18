using SurveyBasket.Api.DTOs.Results;

namespace SurveyBasket.Api.Services
{
    public class ResultService(SurveyBasketDBContext context) : IResultService
    {
        private readonly SurveyBasketDBContext _context = context;

        public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId)
        {
            var polls = await _context.Polls
                .Where(x => x.Id == pollId)
                .Select(p => new PollVotesResponse(
                    p.Title,
                    p.Votes.Select(v => new VoteResponse(
                        $"{v.User.FirstName} {v.User.LastName}",
                        v.SubmittedOn,
                        v.VoteAnswers.Select(a => new QuestionAnswerResponse(
                            a.Question.Content,
                            a.Answer.Content
                            ))
                        ))
                    ))
                    .SingleOrDefaultAsync();

            return polls is null
                ? Result.Failure<PollVotesResponse>(PollErrors.NoPollFound)
                : Result.Success(polls);
        }

        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId)
        {
            var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId);
            if (!pollIsExist)
                return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.NoPollFound);

            var votes = await _context.Votes
                .Where(x => x.PollId == pollId)
                .GroupBy(v => new { date = DateOnly.FromDateTime(v.SubmittedOn) })
                .Select(g => new VotesPerDayResponse(
                    g.Count(),
                    g.Key.date
                    ))
                .ToListAsync();

            return Result.Success<IEnumerable<VotesPerDayResponse>>(votes);
        }
    }
}
