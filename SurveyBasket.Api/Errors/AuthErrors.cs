namespace SurveyBasket.Api.Errors
{
    public static class AuthErrors
    {
        public static Error InValidCredentials => new("User.Credentials", "Invalid Email/Password");
    }
}
