namespace SurveyBasket.Api.DTOs.Questions
{
    public record QuestionRequest(
        string Content ,
        List<string> Answers
        );
}
