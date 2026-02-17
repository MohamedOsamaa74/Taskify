using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.Application.DTOs.UserTeamDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.UserTeamServices;
using Taskify.Domain.Const;

namespace Taskify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTeamController : ControllerBase
    {
        private readonly IUserTeamService _userTeamService;
        public UserTeamController(IUserTeamService userTeamService)
        {
            _userTeamService = userTeamService;
        }
        [Authorize(Roles = $"{UserRole.Admin}, {UserRole.Moderator}")]
        [HttpPost]
        public async Task<Result<UserTeamDTO>> CreateAsync([FromBody] UserTeamDTO userTeamDTO)
        {
            return await _userTeamService.AddUserToTeamAsync(userTeamDTO);
        }

        [Authorize(Roles = $"{UserRole.Admin}, {UserRole.Moderator}")]
        [HttpDelete]
        public async Task<Result<UserTeamDTO>> DeleteAsync([FromBody] UserTeamDTO userTeamDTO)
        {
            return await _userTeamService.RemoveUserFromTeamAsync(userTeamDTO);
        }

        [Authorize]
        [HttpGet]
        public async Task<Result<List<UserTeamDTO>>> GetUserTeamsAsync([FromQuery] Guid? userId)
        {
            return await _userTeamService.GetUserTeamsAsync(userId);
        }
    }
}
