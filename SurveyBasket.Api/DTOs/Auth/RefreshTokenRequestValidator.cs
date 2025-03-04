using FluentValidation;

namespace SurveyBasket.Api.DTOs.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token)
             .NotEmpty();

            RuleFor(x => x.RefreshToken)
             .NotEmpty();
        }
    }
}
