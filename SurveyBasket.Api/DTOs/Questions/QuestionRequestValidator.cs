using FluentValidation;

namespace SurveyBasket.Api.DTOs.Questions
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {

            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.Answers)
                .NotNull();

            RuleFor(x => x.Answers)
                .Must(x => x.Count() > 1)
                .WithMessage("question should has at least 2 answers")
                .When(x => x.Answers != null);

            RuleFor(x => x.Answers)
                .Must(x => x.Count() == x.Distinct().Count())
                .WithMessage("can not be same answer for same question (duplicated answer) ")
                .When(x => x.Answers != null);


        }
    }
}
