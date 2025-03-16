using SurveyBasket.Api.DTOs.Answer;

namespace SurveyBasket.Api.DTOs.Questions
{
    public record   QuestionResponse(
        int Id ,
        string Content,
        IEnumerable<AnswerResponse> Answers
        );
}
