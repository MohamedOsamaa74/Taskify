using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.AccountDTOs;
using Taskify.Application.ResultPattern;

namespace Taskify.Application.Services.AccountServices
{
    public interface IAccountService
    {
        Task<Result<TokenResponseDTO>> RegisterAsync(RegisterUserDTO model);
        Task<Result<TokenResponseDTO>> LoginAsync(LoginUserDTO model);
        Task<bool> LogoutAsync();
        Task<bool> LogoutFromAllDevicesAsync();
        Task<Result<TokenResponseDTO>> RefreshTokenAsync();
        Task<bool> ChangePasswordAsync(ChangePasswordDTO model);
        Task<bool> CreateRoleAsync(string name);
        Task<bool> AddUserToRoleAsync(UserRoleDTO model);
    }
}
