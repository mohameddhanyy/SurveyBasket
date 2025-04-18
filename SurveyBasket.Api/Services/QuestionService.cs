
using Microsoft.Extensions.Caching.Memory;
using SurveyBasket.Api.DTOs.Answer;
using SurveyBasket.Api.Presistance.Models;
using System.Collections.Generic;

namespace SurveyBasket.Api.Services
{
    public class QuestionService(SurveyBasketDBContext context, IMemoryCache memoryCache , ICacheService cacheService) : IQuestionService
    {
        private readonly SurveyBasketDBContext _context = context;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly ICacheService _cacheService = cacheService;
        private const string _cachePrefix = "availableQuestions";


        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId);
            if (!pollIsExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NoPollFound);

            var cacheKey = $"{_cachePrefix}-{pollId}";
            var cachedQuesions = await _cacheService.GetAsync<IEnumerable<QuestionResponse>>(cacheKey);
            IEnumerable<QuestionResponse> questions = [];
            
            if(cachedQuesions is null)
            {
                 questions = await _context.Questions
                            .Where(x => x.PollId == pollId)
                            .Include(x => x.Answers)
                            .ProjectToType<QuestionResponse>()
                            .AsNoTracking()
                            .ToListAsync(cancellationToken: cancellationToken);
                await _cacheService.SetAsync(cacheKey, questions);
            }
            else
            {
                questions = cachedQuesions;
                
            }
            return Result.Success<IEnumerable<QuestionResponse>>(questions!);
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken)
        {
            var hasVote = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId);
            if (hasVote)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteError.DuplicatedVote);

            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow) && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken: cancellationToken);
            if (!pollIsExists)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NoPollFound);

            var cacheKey = $"{_cachePrefix}-{pollId}";
            var question = await _memoryCache.GetOrCreateAsync(
                cacheKey,
                factory =>
                {
                    factory.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    return _context.Questions
                                   .Where(q => q.PollId == pollId && q.IsActive)
                                   .Include(q => q.Answers)
                                   .Select(q => new QuestionResponse
                                   (
                                      q.Id,
                                      q.Content,
                                      q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
                                   ))
                                   .AsNoTracking()
                                   .ToListAsync(cancellationToken: cancellationToken);
                }
                );

            return Result.Success<IEnumerable<QuestionResponse>>(question!);
        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions
                .Where(x => x.PollId == pollId && x.Id == id)
                .Include(x => x.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionError.NoQuestionFound);

            return Result.Success(question);

        }

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);

            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.NoPollFound);

            var questionIsExists = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken: cancellationToken);

            if (questionIsExists)
                return Result.Failure<QuestionResponse>(QuestionError.DuplicatedContent);

            var question = request.Adapt<Question>();
            question.PollId = pollId;

            await _context.Questions.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _memoryCache.Remove($"{_cachePrefix}-available-{pollId}");
            await _cacheService.RemoveAsync($"{_cachePrefix}-available-{pollId}");

            return Result.Success(question.Adapt<QuestionResponse>());
        }

        public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var questionIsExist = await _context.Questions
                .AnyAsync(x => x.PollId == pollId
                               && x.Id != id
                               && x.Content == request.Content
                               , cancellationToken: cancellationToken);
            if (questionIsExist)
                return Result.Failure(QuestionError.DuplicatedContent);

            var question = await _context.Questions
                .Include(x => x.Answers)
                .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id
                , cancellationToken: cancellationToken);
            if (question is null)
                return Result.Failure(QuestionError.NoQuestionFound);

            question.Content = request.Content;

            // current answers 
            var currentAnswers = question.Answers.Select(x => x.Content).ToList();

            // add new answers
            var newAnswers = request.Answers.Except(currentAnswers).ToList();

            foreach (var answer in newAnswers)
                question.Answers.Add(new Answer { Content = answer });

            // check status 
            foreach (var answer in question.Answers.ToList())
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _memoryCache.Remove($"{_cachePrefix}-available-{pollId}");
            await _cacheService.RemoveAsync($"{_cachePrefix}-available-{pollId}");

            return Result.Success();
        }
        public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x => x.Id == id && x.PollId == pollId);
            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionError.NoQuestionFound);

            question.IsActive = !question.IsActive;
            await _context.SaveChangesAsync();
            _memoryCache.Remove($"{_cachePrefix}-available-{pollId}");
            await _cacheService.RemoveAsync($"{_cachePrefix}-available-{pollId}");


            return Result.Success();
        }

    }
}
