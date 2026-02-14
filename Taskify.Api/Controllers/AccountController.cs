using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.Application.DTOs.AccountDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.AccountServices;
using Taskify.Domain.Const;

namespace Taskify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        #region Register
        [HttpPost("register")]
        public async Task<Result<TokenResponseDTO>> Register([FromBody] RegisterUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Result<TokenResponseDTO>.Failure($"Model State Error:{ModelState}");
            }
            var result = await _accountService.RegisterAsync(model);
            return result;
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<Result<TokenResponseDTO>> LoginAsync([FromBody] LoginUserDTO model)
        {
            var result = await _accountService.LoginAsync(model);
            return result;
        }
        #endregion

        #region Logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<Result<TokenResponseDTO>> LogoutAsync()
        {
            var result = await _accountService.LogoutAsync();
            return result;
        }
        #endregion

        #region Logout From All Devices
        [Authorize]
        [HttpPost("logoutFromAllDevices")]
        public async Task<Result<TokenResponseDTO>> LogoutFromAllDevicesAsync()
        {
            var result = await _accountService.LogoutFromAllDevicesAsync();
            return result;
        }
        #endregion

        #region RefreshToken
        [HttpPost("refreshToken")]
        public async Task<Result<TokenResponseDTO>> RefreshTokenAsync()
        {
            var result = await _accountService.RefreshTokenAsync();
            return result;
        }
        #endregion

        #region Change Password
        [Authorize]
        [HttpPost("changePassword")]
        public async Task<Result<TokenResponseDTO>> ChangePasswordAsync([FromBody] ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Result<TokenResponseDTO>.Failure($"Model State Error:{ModelState}");
            }
            var result = await _accountService.ChangePasswordAsync(model);
            return result;
        }
        #endregion

        #region Create Role
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("Create Role")]
        public async Task<Result<TokenResponseDTO>> CreateRoleAsync(string Name)
        {
            var result = await _accountService.CreateRoleAsync(Name);
            return result;
        }
        #endregion

        #region Add User to Role
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("AddUserToRole")]
        public async Task<Result<TokenResponseDTO>> AddUserToRoleAsync(UserRoleDTO model)
        {
            if (!ModelState.IsValid)
                return Result<TokenResponseDTO>.Failure($"Model State Error:{ModelState}");
            var result = await _accountService.AddUserToRoleAsync(model);
            return result;
        }
        #endregion
    }
}
