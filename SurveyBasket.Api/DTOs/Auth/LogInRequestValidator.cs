using FluentValidation;
using FluentValidation.Results;

namespace SurveyBasket.Api.DTOs.Auth
{
    public class LogInRequestValidator : AbstractValidator<LogInRequest>
    {
        public LogInRequestValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

            RuleFor(x => x.Password)
             .NotEmpty();
        }


    }
}
