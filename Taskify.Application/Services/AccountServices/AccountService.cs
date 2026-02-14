using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Taskify.Application.DTOs.AccountDTOs;
using Taskify.Application.Helpers;
using Taskify.Application.ResultPattern;
using Taskify.Domain.Const;
using Taskify.Domain.Entities;
using Taskify.Infrastructure.Contexts;

namespace Taskify.Application.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JWT> _jwt;
        public AccountService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole>roleManager, IHttpContextAccessor httpContextAccessor, IOptions<JWT> jwt)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _jwt = jwt;
        }

        #region Register
        public async Task<Result<TokenResponseDTO>> RegisterAsync(RegisterUserDTO model)
        {
            try
            {

                if (await GetUserbyUserNameAsync(model.UserName) is not null
                    || await GetUserByEmailAsync(model.Email) is not null)
                {
                    return Result<TokenResponseDTO>.Failure("User Already Exist");
                }
                if (model.Password != model.ConfirmPassword)
                {
                    return Result<TokenResponseDTO>.Failure("Password Don't match");
                }
                    var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    FullName = model.FullName,
                    UserName = model.UserName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return Result<TokenResponseDTO>.Failure($"an errorr occured: {result.Errors}");
                }
                await _userManager.AddToRoleAsync(user, UserRole.User);
                var tokenResponse = await GenerateTokenResponseAsync(user);
                await SaveUserRefreshTokenAsync(user, tokenResponse.RefreshToken);
                SetRefreshTokenInCookie(tokenResponse.RefreshToken.Token, tokenResponse.RefreshToken.ExpiresOn);
                return Result<TokenResponseDTO>.Created(tokenResponse);
            }
            catch(Exception ex)
            {
                return Result<TokenResponseDTO>.Failure($"an errorr occured: {ex}");
            }
        }
        #endregion

        #region Login
        public async Task<Result<TokenResponseDTO>> LoginAsync(LoginUserDTO model)
        {
            try
            {

                var user = await GetUserByEmailAsync(model.LoginIdentifier) ?? await GetUserbyUserNameAsync(model.LoginIdentifier);
                if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Result<TokenResponseDTO>.Failure("Invalid Credintials");
                TokenResponseDTO tokenResponse;
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive == true);
                if (activeRefreshToken is not null)
                {
                    var jwtToken = await CreateTokenAsync(user);
                    tokenResponse = new TokenResponseDTO
                    {
                        Token = jwtToken,
                        RefreshToken = activeRefreshToken
                    };
                }
                else
                {
                    tokenResponse = await GenerateTokenResponseAsync(user);
                    await SaveUserRefreshTokenAsync(user, tokenResponse.RefreshToken);
                }
                SetRefreshTokenInCookie(tokenResponse.RefreshToken.Token, tokenResponse.RefreshToken.ExpiresOn);
                return Result<TokenResponseDTO>.Success(tokenResponse);
            }
            catch (Exception ex)
            {
                return Result<TokenResponseDTO>.Failure($"an errorr occured: {ex}");
            }
        }
        #endregion

        #region Logout
        public async Task<bool> LogoutAsync()
        {
            var token = GetRefreshTokenFromCookie();
            if (string.IsNullOrEmpty(token))
                return false;
            var result = await RevokeRefreshTokenAsync(token);
            if (result)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            }
            return result;
        }
        #endregion

        #region Logout From All Devices
        public async Task<bool> LogoutFromAllDevicesAsync()
        {
            var token = GetRefreshTokenFromCookie();
            if (string.IsNullOrEmpty(token))
                return false;
            var user = await _userManager.Users.SingleOrDefaultAsync
                       (u => u.RefreshTokens.Any(t => t.Token == token));
            if (user is null)
                return false;
            var result = await RevokeAllUserTokensAsync(user.Id.ToString());
            if (result)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            }
            return result;
        }
        #endregion

        #region Refresh Token
        public async Task<Result<TokenResponseDTO>> RefreshTokenAsync()
        {
            var refreshToken = GetRefreshTokenFromCookie();
            if (string.IsNullOrEmpty(refreshToken))
                return null;
            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync
                       (u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user is null)
                return null;
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
            {
                await RevokeAllUserTokensAsync(user.Id.ToString());
                return null;
            }
            oldRefreshToken.RevokedOn = DateTime.UtcNow;
            var tokenResponse = await GenerateTokenResponseAsync(user);
            var newRefreshToken = tokenResponse.RefreshToken;
            await SaveUserRefreshTokenAsync(user, newRefreshToken);
            SetRefreshTokenInCookie(newRefreshToken.Token, newRefreshToken.ExpiresOn);
            await CleanUpExpiredTokensAsync(user);
            return Result<TokenResponseDTO>.Success(tokenResponse);
        }
        #endregion

        #region Change Password
        public async Task<bool> ChangePasswordAsync(ChangePasswordDTO model)
        {
            var user = await GetCurrentUserAsync();
            if (user is null || model.NewPassword != model.ConfirmNewPassword)
                return false;
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await RevokeAllUserTokensAsync(user.Id.ToString());
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
                return true;
            }
            return false;
        }
        #endregion

        #region Create Role
        public async Task<bool> CreateRoleAsync(string name)
        {
            if (await _roleManager.FindByNameAsync(name) != null)
                return false;
            var role = new ApplicationRole
            {
                Name = name,
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return false;
            return true;
        }
        #endregion

        #region Add User to Role
        public async Task<bool> AddUserToRoleAsync(UserRoleDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
                return false;
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role is null)
                return false;
            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return false;
            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
                return false;
            return true;
        }
        #endregion

        #region private methods

        #region Get Current User
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            ClaimsPrincipal claims = _httpContextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(claims);
        }
        #endregion

        #region Generate Token Response
        private async Task<TokenResponseDTO> GenerateTokenResponseAsync(ApplicationUser user)
        {
            var token = await CreateTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            return new TokenResponseDTO { Token = token, RefreshToken = refreshToken };
        }
        #endregion

        #region Create JWT Token
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                //Token Id
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new
            (
                issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.Value.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Check if user exist
        private async Task<ApplicationUser> GetUserbyUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }
        #endregion

        #region Check if email exist
        private async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        #endregion

        #region Generate Refresh Token
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
            };
        }
        #endregion

        #region Save Refresh Token
        private async Task SaveUserRefreshTokenAsync(ApplicationUser user, RefreshToken refreshToken)
        {
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
        }
        #endregion

        #region Get Refresh Token from Cookie
        private string GetRefreshTokenFromCookie()
            => _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
        #endregion

        #region Set Refresh Token in Cockie
        private void SetRefreshTokenInCookie(string token, DateTime expiry)
        {
            var CockieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expiry,
                SameSite = SameSiteMode.Strict
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", token, CockieOptions);
        }
        #endregion

        #region Revoke Refresh Token
        private async Task<bool> RevokeRefreshTokenAsync(string Token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync
                        (u => u.RefreshTokens.Any(t => t.Token == Token));
            if (user == null)
                return false;
            var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
            if (!refreshToken.IsActive)
                return false;
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }
        #endregion

        #region Revoke All User Tokens
        private async Task<bool> RevokeAllUserTokensAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
            {
                token.RevokedOn = DateTime.UtcNow;
            }

            await _userManager.UpdateAsync(user);

            return true;
        }
        #endregion

        #region Cleaning Expired Tokens
        private async Task<bool> CleanUpExpiredTokensAsync(ApplicationUser user)
        {
            var expiredTokens = user.RefreshTokens.Where(t => !t.IsActive && t.RevokedOn.HasValue
                                && t.RevokedOn.Value.AddDays(30) < DateTime.UtcNow).ToList();
            if (!expiredTokens.Any())
                return false;
            user.RefreshTokens.RemoveAll(t => expiredTokens.Contains(t));
            await _userManager.UpdateAsync(user);
            return true;
        }
        #endregion

        #endregion
    }
}
