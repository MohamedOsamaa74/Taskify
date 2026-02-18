using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskify.Application.DTOs.TaskDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.TaskItemService;

namespace Taskify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;

        [Authorize]
        [HttpPost]
        public async Task<Result<TaskDTO>> CreateAsync(CreateTaskDTO createTaskDTO)
        {
            if (!ModelState.IsValid)
            {
                return Result<TaskDTO>.Failure($"Model State Error, {ModelState}");
            }
            return await _taskService.CreateAsync(createTaskDTO);
        }

        [HttpGet("{id}")]
        public async Task<Result<TaskDTO>> GetByIdAsync(int id)
        {
            return await _taskService.GetByIdAsync(id);
        }

        [Authorize]
        [HttpGet("ToDoList/{id}")]
        public async Task<Result<List<TaskDTO>>> GetByListIDAsync(int id)
        {
            return await _taskService.GetByListIdAsync(id);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<Result<TaskDTO>> UpdateAsync(int id, CreateTaskDTO updateTaskDTO)
        {
            if (!ModelState.IsValid)
            {
                return Result<TaskDTO>.Failure($"Model State Error, {ModelState}");
            }
            return await _taskService.UpdateAsync(id, updateTaskDTO);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<Result<TaskDTO>> DeleteAsync(int id)
        {
            return await _taskService.DeleteAsync(id);
        }

        [HttpPut("MarkAsDone/{id}")]
        public async Task<Result<TaskDTO>> MarkAsDoneAsync(int id)
        {
            return await _taskService.MarkAsDoneAsync(id);
        }
    }
}
