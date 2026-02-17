using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Taskify.Application.DTOs.TaskDTOs;
using Taskify.Application.ResultPattern;

namespace Taskify.Application.Services.TaskItemService
{
    public interface ITaskService
    {
        Task<Result<TaskDTO>> CreateAsync(CreateTaskDTO createTaskDTO);
        Task<Result<TaskDTO>> GetByIdAsync(int id);
        Task<Result<List<TaskDTO>>> GetByListIdAsync(int id);
        Task<Result<TaskDTO>> UpdateAsync(int id, CreateTaskDTO updateTaskDTO);
        Task<Result<TaskDTO>> DeleteAsync(int id);
        Task<Result<TaskDTO>> MarkAsDoneAsync(int id);
    }
}
