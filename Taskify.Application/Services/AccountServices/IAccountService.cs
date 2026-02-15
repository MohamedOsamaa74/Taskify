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
        Task<Result<TokenResponseDTO>> LogoutAsync();
        Task<Result<TokenResponseDTO>> LogoutFromAllDevicesAsync();
        Task<Result<TokenResponseDTO>> RefreshTokenAsync();
        Task<Result<TokenResponseDTO>> ChangePasswordAsync(ChangePasswordDTO model);
        Task<Result<TokenResponseDTO>> CreateRoleAsync(string name);
        Task<Result<TokenResponseDTO>> AddUserToRoleAsync(UserRoleDTO model);
        Task<Result<Guid>> GetCurrentUserId();
    }
}
