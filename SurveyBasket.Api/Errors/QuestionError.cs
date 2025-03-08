namespace SurveyBasket.Api.Errors
{
    public class QuestionError
    {
        public static Error NoQuestionFound => new("Question.NotFound ", "no Question with given Id");
        public static Error DuplicatedContent => new("Content.Duplicated ", "can not be duplicated Content ");

    }
}
