
namespace SurveyBasket.Api.ServiceContracts
{
    public interface IQuestionService
    {
        Task<Result<QuestionResponse>> GetAsync(int pollId,int id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken);
        Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken);

        Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken);

        Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken);

    }
}
