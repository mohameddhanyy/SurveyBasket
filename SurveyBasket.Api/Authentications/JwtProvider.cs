using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Api.Presistance.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Authentications
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = [
                new Claim(JwtRegisteredClaimNames.Sub , user.Id),
                new Claim(JwtRegisteredClaimNames.Email , user.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName , user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
                ];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var expiresIn = _options.ExpiryMinute * 60;

            var token = new JwtSecurityToken(
                audience: _options.Audience,
                issuer: _options.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresIn),
                signingCredentials: signInCredentials
                );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn);
        }
    }
}
