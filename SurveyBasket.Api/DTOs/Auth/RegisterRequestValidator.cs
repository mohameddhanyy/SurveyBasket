using FluentValidation;
using SurveyBasket.Api.Abstractions.Consts;

namespace SurveyBasket.Api.DTOs.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

            RuleFor(x => x.Password)
             .NotEmpty()
             .Matches(RegexPattern.Password)
             .WithMessage("password must be more than 8 and should have uppercases and lowercases");

            RuleFor(x => x.FirstName)
             .NotEmpty()
             .Length(3,100);

            RuleFor(x => x.LastName)
             .NotEmpty()
             .Length(3, 100);
        }

    }
}
