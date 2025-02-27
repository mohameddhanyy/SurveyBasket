using FluentValidation;
using FluentValidation.Results;
using SurveyBasket.Api.DTOs.Requests;

namespace SurveyBasket.Api.Helpers.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty()
             .Length(3, 15);
            RuleFor(x => x.Description)
             .NotEmpty()
             .Length(3, 100);

        }

    }
}
