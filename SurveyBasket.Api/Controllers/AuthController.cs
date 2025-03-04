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
        public async Task<IActionResult> LogInAsync(LogInRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResult is null ? BadRequest() : Ok(authResult);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return Ok(authResult);

        }
    }
}
