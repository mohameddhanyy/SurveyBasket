using FluentValidation;

namespace SurveyBasket.Api.DTOs.Vote
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest> 
    {
        public VoteRequestValidator()
        {
            RuleFor(x => x.Answers)
                .NotEmpty();
        }
    }
}
