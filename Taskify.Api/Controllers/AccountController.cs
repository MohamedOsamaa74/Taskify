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
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _accountService.LogoutAsync();
            if (!result)
                return BadRequest("Logout failed");
            return Ok("Logged out successfully");
        }
        #endregion

        #region Logout From All Devices
        [Authorize]
        [HttpPost("logoutFromAllDevices")]
        public async Task<IActionResult> LogoutFromAllDevicesAsync()
        {
            var result = await _accountService.LogoutFromAllDevicesAsync();
            if (!result)
                return BadRequest("Logout From All Devices failed");
            return Ok("Logged out From All Devices successfully");
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
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.ChangePasswordAsync(model);
            if (!result)
                return BadRequest("Password change failed");
            return Ok("Password changed successfully, Please login Again");
        }
        #endregion

        #region Create Role
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("Create Role")]
        public async Task<IActionResult> CreateRoleAsync(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return BadRequest("Name must be Provided");
            var result = await _accountService.CreateRoleAsync(Name);
            if (!result)
                return BadRequest();
            return Ok("Role Created Successfully");
        }
        #endregion

        #region Add User to Role
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRoleAsync(UserRoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _accountService.AddUserToRoleAsync(model);
            if (!result)
                return BadRequest("Failed To Add User To Role");
            return Ok("Added User To Role Successfully");
        }
        #endregion
    }
}
