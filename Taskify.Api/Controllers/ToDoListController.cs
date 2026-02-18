using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.Application.DTOs.ToDoListDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.ToDoListServices;

namespace Taskify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController(IToDoListService toDoListService) : ControllerBase
    {
        private readonly IToDoListService _toDoListService = toDoListService;

        [Authorize]
        [HttpPost]
        public async Task<Result<ToDoListDTO>> CraeteAsync([FromBody] CreateToDoListDTO createToDoListDTO)
        {
            if (!ModelState.IsValid)
            {
                return Result<ToDoListDTO>.Failure($"Invalid Model State,{ModelState}");
            }
            return await _toDoListService.CreateAsync(createToDoListDTO);
        }

        [HttpGet("{id}")]
        public async Task<Result<ToDoListDTO>> GetByIdAsync(int id)
        {
            return await _toDoListService.GetByIdAsync(id);
        }

        [HttpGet("UserId/{id}")]
        public async Task<Result<List<ToDoListDTO>>> GetByUserIdAsync(Guid id)
        {
            return await _toDoListService.GetUserToDoListsAsync(id);
        }

        [HttpGet("TeamId/{id}")]
        public async Task<Result<List<ToDoListDTO>>> GetByTeamIdAsync(int id)
        {
            return await _toDoListService.GetTeamToDoListsAsync(id);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<Result<ToDoListDTO>> DeleteAsync(int id)
        {
            return await _toDoListService.DeleteAsync(id);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<Result<ToDoListDTO>> UpdateAsync(int id, [FromBody] CreateToDoListDTO createToDoListDTO)
        {
            if (!ModelState.IsValid)
            {
                return Result<ToDoListDTO>.Failure($"Invalid Model State,{ModelState}");
            }
            return await _toDoListService.UpdateAsync(id, createToDoListDTO);
        }
    }
}
