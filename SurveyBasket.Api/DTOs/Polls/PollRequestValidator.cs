using FluentValidation;
using FluentValidation.Results;

namespace SurveyBasket.Api.DTOs.Polls
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Title)
             .NotEmpty()
             .Length(3, 15);

            RuleFor(x => x.Summary)
             .NotEmpty()
             .Length(3, 1500);

            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x)
                .Must(HasValidDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("End-Date must not be greater than Start-Date");
        }

        private bool HasValidDate(PollRequest poll)
            => poll.EndsAt >=  poll.StartsAt;

    }
}
