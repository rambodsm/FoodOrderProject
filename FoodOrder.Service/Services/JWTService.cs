using FoodOrder.Common.Enums;
using FoodOrder.Common.Result;
using FoodOrder.Common.SiteSettings;
using FoodOrder.Common.Utilities;
using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Infrastructure.UnitOfWork;
using FoodOrder.Presentation.Models.UserViewModels;
using FoodOrder.Service.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Service.Services
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSetting;
        private readonly SignInManager<User> _signInManager;
        private readonly IRefreshTokenRepository _refreshToken;
        private readonly IUnitOfWork _uow;
        private SymmetricSecurityKey SecretKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey));
        private SymmetricSecurityKey EncryptionKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey));

        public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager, IRefreshTokenRepository refreshToken, IUnitOfWork uow)
        {
            _siteSetting = settings.Value;
            _signInManager = signInManager;
            _refreshToken = refreshToken;
            _uow = uow;
        }

        #region Generate Token

        public async Task<AccessToken> GenerateAsync(User user)
        {
            var accessToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(await _getSecurityTokenConfigurationAsync(user));
            var refreshToken = await _generateRefreshToken(user.Id, accessToken.Id);
            if (refreshToken.Success is false)
                return new AccessToken(accessToken);
            return new AccessToken(accessToken, refreshToken.Result);
        }
        private async Task<SecurityTokenDescriptor> _getSecurityTokenConfigurationAsync(User user)
        {
            var signingCredentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256Signature);
            var encryptingCredentials = new EncryptingCredentials(EncryptionKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            var claims = await _getClaimsAsync(user);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };
            return descriptor;
        }
        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
            var list = new List<Claim>(result.Claims);
            list.Add(new Claim(IdentityField.UserId.ToString(), user.Id.ToString()));
            list.Add(new Claim(ClaimTypes.Name, user.UserName.ToString()));
            list.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber.ToString()));
            list.Add(new Claim(ClaimTypes.Email, user.Email));
            list.Add(new Claim(securityStampClaimType, user.SecurityStamp.ToString()));
            list.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            //TODO:Get UserRole
            //var roles = new Role[] { new Role { Name = "Admin" } };
            //foreach (var role in roles)
            //    list.Add(new Claim(ClaimTypes.Role, role.Name));
            return list;
        }
        private async Task<OperationResult<string>> _generateRefreshToken(Guid userId, string tokenId)
        {
            var refreshToken = new RefreshToken
            {
                JwtId = tokenId,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = SecurityHelper.GeneratePassword(35, 5) + Guid.NewGuid()
            };
            var result = await _addRefreshToken(refreshToken);
            if (result.Success is false)
                return OperationResult<string>.BuildFailure("خطا در ذخیره سازی refreshToken");
            return OperationResult<string>.BuildSuccessResult(refreshToken.Token);
        }
        private async Task<OperationResult> _addRefreshToken(RefreshToken refreshToken)
        {
            await _refreshToken.AddAsync(refreshToken, CancellationToken.None);
            return await _uow.CommitAsync();
        }

        #endregion

        #region Verify And GenerateToken

        public async Task<OperationResult<AccessToken>> VerifyAndGenerateAsync(TokenRequestViewModel tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecretKey,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = _siteSetting.JwtSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _siteSetting.JwtSettings.Issuer,
                TokenDecryptionKey = EncryptionKey
            };
            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, validationParameters, out var validatedToken);
            var validationTokenResult = await _validationTokenAsync(tokenInVerification, validatedToken, tokenRequest);
            if (validationTokenResult.Success is false)
                return OperationResult<AccessToken>.BuildFailure(validationTokenResult.ErrorMessage);
            var revokeRefreshTokenResult = await _revokeRefreshTokenAsync(validationTokenResult.Result);
            if (revokeRefreshTokenResult.Success is false)
                return OperationResult<AccessToken>.BuildFailure("Error on revoke token operation");
            var user = await _getUserByUserIdAsync(validationTokenResult.Result.UserId.Value);
            if (user.Success is false)
                return OperationResult<AccessToken>.BuildFailure(user.ErrorMessage);
            return OperationResult<AccessToken>.BuildSuccessResult(await GenerateAsync(user.Result));
        }
        private async Task<OperationResult<RefreshToken>> _validationTokenAsync(ClaimsPrincipal tokenInVerification, SecurityToken validatedToken, TokenRequestViewModel tokenRequest)
        {
            if (validatedToken is not JwtSecurityToken jwtSecurityToken)
                return OperationResult<RefreshToken>.BuildFailure("Token in not valid");
            var tokenSecurityAlgResult = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase);
            if (tokenSecurityAlgResult is false)
                return OperationResult<RefreshToken>.BuildFailure("Token is not valid");
            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            if (_unixTimeStampToDateTime(utcExpiryDate) > DateTime.UtcNow)
                return OperationResult<RefreshToken>.BuildFailure("Token has not yet expired");
            var storedToken = await _refreshToken.TableNoTracking.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
            if (storedToken is null)
                return OperationResult<RefreshToken>.BuildFailure("Token doesn't Exist");
            if (storedToken.IsUsed is true)
                return OperationResult<RefreshToken>.BuildFailure("Token has been used");
            if (storedToken.IsRevorked is true)
                return OperationResult<RefreshToken>.BuildFailure("Token has been revoked");
            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedToken.JwtId != jti)
                return OperationResult<RefreshToken>.BuildFailure("Token doesn't match");
            return OperationResult<RefreshToken>.BuildSuccessResult(storedToken);
        }
        private DateTime _unixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTimeVal;
        }
        private async Task<OperationResult> _revokeRefreshTokenAsync(RefreshToken token)
        {
            token.IsUsed = true;
            _refreshToken.Update(token);
            return await _uow.CommitAsync();
        }
        private async Task<OperationResult<User>> _getUserByUserIdAsync(Guid userId)
        {
            var user = await _refreshToken.TableNoTracking.Where(p => p.User.Id == userId).Select(p => p.User).SingleOrDefaultAsync();
            if (user is null)
                return OperationResult<User>.BuildFailure("User not Found");
            return OperationResult<User>.BuildSuccessResult(user);
        }

        #endregion
    }
}
