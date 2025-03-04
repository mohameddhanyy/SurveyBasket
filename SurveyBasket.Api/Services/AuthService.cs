﻿using Microsoft.AspNetCore.Identity;
using SurveyBasket.Api.Authentications;
using SurveyBasket.Api.DTOs.Auth;
using SurveyBasket.Api.Presistance.Models;
using SurveyBasket.Api.ServiceContracts;
using System.Security.Cryptography;

namespace SurveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpireyDays = 14;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
                return null;

            var (token, expireIn) = _jwtProvider.GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireyDays);

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);
            return new AuthResponse(user.Id , user.Email,user.FirstName,user.LastName,token,expireIn,refreshToken,refreshTokenExpiration);
        }


        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null) return null!;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return null!;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null) return null;

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


        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
