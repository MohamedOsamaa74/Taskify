using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.ToDoListDTOs;
using Taskify.Application.ResultPattern;

namespace Taskify.Application.Services.ToDoListServices
{
    public interface IToDoListService
    {
        Task<Result<ToDoListDTO>> CreateAsync(CreateToDoListDTO createToDoListDTO);
        Task<Result<ToDoListDTO>> GetByIdAsync(int id);
        Task<Result<List<ToDoListDTO>>> GetUserToDoListsAsync(Guid userId);
        Task<Result<List<ToDoListDTO>>> GetTeamToDoListsAsync(int teamId);
        Task<Result<ToDoListDTO>> DeleteAsync(int id);
        Task<Result<ToDoListDTO>> UpdateAsync(int id, CreateToDoListDTO createToDoListDTO);
    }
}
