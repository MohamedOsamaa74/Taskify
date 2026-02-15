using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.TeamDTOs;
using Taskify.Application.ResultPattern;

namespace Taskify.Application.Services.TeamServices
{
    public interface ITeamService
    {
        Task<Result<TeamDTO>> CreateAsync(CreateTeamDTO createTeamDTO);
        Task<Result<TeamDTO>> GetByIdAsync(int id);
        Task<Result<List<TeamDTO>>> GetAllAsync();
        Task<Result<TeamDTO>> UpdateAsync(int id, CreateTeamDTO updateTeamDTO);
        Task<Result<TeamDTO>> DeleteAsync(int id);

    }
}
