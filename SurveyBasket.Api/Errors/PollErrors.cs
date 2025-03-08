namespace SurveyBasket.Api.Errors
{
    public static class PollErrors
    {
        public static Error NoPollFound => new("Poll.NotFound ", "no poll with given Id");
        public static Error DuplicatedTitle => new("Title.Duplicated ", "can not be duplicated title ");
    }
}
