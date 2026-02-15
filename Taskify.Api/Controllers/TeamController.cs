using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.Application.DTOs.TeamDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.TeamServices;
using Taskify.Domain.Const;

namespace Taskify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [Authorize]
        [HttpPost]
        public async Task<Result<TeamDTO>> CreateAsync([FromBody] CreateTeamDTO createTeamDTO)
        {
            return await _teamService.CreateAsync(createTeamDTO);
        }

        [HttpGet("{id}")]
        public async Task<Result<TeamDTO>> GetByIdAsync(int id)
        {
            return await _teamService.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<Result<List<TeamDTO>>> GetAllAsync()
        {
            return await _teamService.GetAllAsync();
        }

        [Authorize(Roles = $"{UserRole.Admin},{UserRole.Moderator}")]
        [HttpPut("{id}")]
        public async Task<Result<TeamDTO>> UpdateAsync(int id, [FromBody] CreateTeamDTO updateTeamDTO)
        {
            return await _teamService.UpdateAsync(id, updateTeamDTO);
        }

        [Authorize(Roles = $"{UserRole.Admin},{UserRole.Moderator}")]
        [HttpDelete("{id}")]
        public async Task<Result<TeamDTO>> DeleteAsync(int id)
        {
            return await _teamService.DeleteAsync(id);
        }
    }
}
