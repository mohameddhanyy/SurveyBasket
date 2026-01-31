namespace SurveyBasket.Api.DTOs.Auth
{
    public class ResendConfirmationEmailValidator : AbstractValidator<ResendConfirmationEmailRequest>
    {
        public ResendConfirmationEmailValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();
        }
    }
}
