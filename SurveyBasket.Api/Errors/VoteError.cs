namespace SurveyBasket.Api.Errors
{
    public class VoteError
    {
        public static Error DuplicatedVote => new("Vote.Duplicate ", "can not be voted more than one");
        public static Error InCorrectQuestions => new("Questions.NOTCorrect ", "can not be voted with this questions ");

    }
}
