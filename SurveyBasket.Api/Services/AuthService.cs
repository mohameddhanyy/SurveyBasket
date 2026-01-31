using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Authentications;
using SurveyBasket.Api.DTOs.Auth;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Helpers.Email;
using SurveyBasket.Api.Presistance.Models;
using SurveyBasket.Api.ServiceContracts;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AuthService> logger,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly int _refreshTokenExpireyDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Failure<AuthResponse>(AuthErrors.InValidCredentials);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
            {
                var (token, expireIn) = _jwtProvider.GenerateToken(user);

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireyDays);

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn, refreshToken, refreshTokenExpiration);
                return Result.Success(response);
            }
            return Result.Failure<AuthResponse>(result.IsNotAllowed ? AuthErrors.EmailNOTConfirmed : AuthErrors.InValidCredentials);
        }


        public async Task<AuthResponse> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null) return null!;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return null!;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null) return null!;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newToken, expireIn) = _jwtProvider.GenerateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireyDays);

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newRefreshToken, expireIn, newRefreshToken, refreshTokenExpiration);
        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var emailExist = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (emailExist)
                return Result.Failure(AuthErrors.DuplicatedEmail);

            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Email confirmation code for user {UserId} : {Code}", user.Id, code);

                await SendConfirmationEmail(user,code);

                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description));
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(AuthErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.DuplicatedConfirmedEmail);

            var code = request.Code;
            try
            {
                 code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch
            {
                return Result.Failure(AuthErrors.InvalidCode);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description));
        }

        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();
            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.DuplicatedConfirmedEmail);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Email confirmation code for user {UserId} : {Code}", user.Id, code);
            await SendConfirmationEmail(user, code);
            return Result.Success();
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
                var origin = _httpContextAccessor.HttpContext?.Request.Headers["Origin"].ToString() ?? string.Empty;

                var emailBody = EmailBodyHelper.GenerateEmailBody("ConfirmEmail", new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/Auth/EmailConfirmation?userId={user.Id}&code={code}" }
                });

                await _emailSender.SendEmailAsync(user.Email!, "✅Survey Basket : Confirm your email", emailBody);
        }
    }
}
