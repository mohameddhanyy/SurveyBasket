using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Api.Authentications;
using SurveyBasket.Api.DTOs.Auth;
using SurveyBasket.Api.ServiceContracts;

namespace SurveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService , IOptions<JwtOptions> options) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly JwtOptions _options = options.Value;

        [HttpPost("")]
        public async Task<IActionResult> LogIn(LogInRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResult.IsSuccess ?
                Ok(authResult.Value): Unauthorized(authResult.Error);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return Ok(authResult);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var registerResult = await _authService.RegisterAsync(request, cancellationToken);
            return registerResult.IsSuccess ?
                Ok() : BadRequest(registerResult.Error);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> EmailConfirm(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var registerResult = await _authService.ConfirmEmailAsync(request);
            return registerResult.IsSuccess ?
                Ok() : BadRequest(registerResult.Error);
        }
        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmationEmail(ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
        {
            var registerResult = await _authService.ResendConfirmationEmailAsync(request);
            return registerResult.IsSuccess ?
                Ok() : BadRequest(registerResult.Error);
        }
    }
}
