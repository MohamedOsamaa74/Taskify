using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.UserTeamDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Domain.Entities;

namespace Taskify.Application.Services.UserTeamServices
{
    public interface IUserTeamService
    {
        Task<Result<UserTeamDTO>> AddUserToTeamAsync(UserTeamDTO userTeamDTO);
        Task<Result<UserTeamDTO>> RemoveUserFromTeamAsync(UserTeamDTO userTeamDTO);
        Task<Result<List<UserTeamDTO>>> GetUserTeamsAsync(Guid? userId);
    }
}
