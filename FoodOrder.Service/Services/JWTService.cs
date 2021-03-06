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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager, IRefreshTokenRepository refreshToken, IUnitOfWork uow)
        {
            _siteSetting = settings.Value;
            _signInManager = signInManager;
            _refreshToken = refreshToken;
            _uow = uow;
        }

        public async Task<AccessToken> GenerateAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey);
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            var refreshToken = await _generateRefreshToken(user.Id, securityToken.Id);
            if (refreshToken.Success is false)
                return new AccessToken(securityToken);
            return new AccessToken(securityToken, refreshToken.Result);
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
    }
}
