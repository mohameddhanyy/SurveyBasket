namespace SurveyBasket.Api.Abstractions.Consts
{
    public class RegexPattern
    {
        public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$";
    }
}
