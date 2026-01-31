namespace SurveyBasket.Api.Errors
{
    public static class AuthErrors
    {
        public static Error InValidCredentials => new("User.Credentials", "Invalid Email/Password");
        public static Error DuplicatedEmail => new("Email.Conflict", "Another user already have same email ");
        public static Error EmailNOTConfirmed => new("User.NotCofirmedEmail", "user need to confirm email");
        public static Error InvalidCode => new("User.InvalidCode", "Invalid Code");
        public static Error DuplicatedConfirmedEmail => new("User.DuplicatedConfirm", "user already confirm his email");

    }
}
