using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.TaskDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.TaskItemService;
using Taskify.Domain.Entities;
using Taskify.Domain.Repositories;

namespace Taskify.Application.Services.TaskServices
{
    public class TaskService(IUnitOfWork unitOfWork) : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        
        #region Create
        public async Task<Result<TaskDTO>> CreateAsync(CreateTaskDTO createTaskDTO)
        {
            try
            {
                var toDoList = await _unitOfWork.ToDoListRepository.GetByIdAsync(createTaskDTO.ToDoListId);
                if(toDoList is null)
                    return Result<TaskDTO>.Failure("To Do List does not exist");
                TaskItem task = new()
                {
                    Title = createTaskDTO.Title,
                    Description = createTaskDTO.Description,
                    IsDone = createTaskDTO.IsDone,
                    ToDoListId = createTaskDTO.ToDoListId,
                };
                await _unitOfWork.TaskItemRepository.AddAsync(task);
                await _unitOfWork.SaveChangesAsync();
                TaskDTO taskDTO = new()
                {
                    Title = createTaskDTO.Title,
                    Description = createTaskDTO.Description,
                    IsDone = createTaskDTO.IsDone,
                    ToDoListId = createTaskDTO.ToDoListId,
                    ToDoListName = toDoList.Name
                };
                return Result<TaskDTO>.Success(taskDTO);
            }
            catch(Exception ex)
            {
                return Result<TaskDTO>.Failure($"An error occurred during creating the task, {ex}");
            }
        }
        #endregion

        #region Get By Id
        public async Task<Result<TaskDTO>> GetByIdAsync(int id)
        {
            try
            {
                var task = await _unitOfWork.TaskItemRepository
                    .GetAll(t => t.Id == id)
                    .Include(t => t.ToDoList)
                    .FirstOrDefaultAsync();
                    
                if(task is null)
                    return Result<TaskDTO>.NotFound("the task does not exist");
                    
                TaskDTO taskDTO = new()
                {
                    Title = task.Title,
                    Description = task.Description,
                    IsDone = task.IsDone,
                    ToDoListId = task.ToDoListId,
                    ToDoListName = task.ToDoList.Name
                };
                return Result<TaskDTO>.Success(taskDTO);
            }
            catch(Exception ex)
            {
                return Result<TaskDTO>.Failure($"an error occurred while fetching the task, {ex}");
            }

        }
        #endregion

        #region Get By To Do List Id
        public async Task<Result<List<TaskDTO>>> GetByListIdAsync(int id)
        {
            try
            {
                var toDoList = await _unitOfWork.ToDoListRepository.GetByIdAsync(id);
                if(toDoList is null)
                {
                    return Result<List<TaskDTO>>.NotFound("List does not exist");
                }
                var tasksDTO = await _unitOfWork.TaskItemRepository
                    .GetAll(t => t.ToDoListId == id)
                    .Include(tt => tt.ToDoList)
                    .Select(ti => new TaskDTO { Title = ti.Title, Description = ti.Description, IsDone = ti.IsDone, ToDoListId = id, ToDoListName = ti.ToDoList.Name })
                    .ToListAsync();
                return Result<List<TaskDTO>>.Success(tasksDTO);
            }
            catch (Exception ex)
            {
                return Result<List<TaskDTO>>.Failure($"an error occurred while fetching the List tasks, {ex}");
            }
        }
        #endregion

        #region Update
        public async Task<Result<TaskDTO>> UpdateAsync(int id, CreateTaskDTO updateTaskDTO)
        {
            try
            {
                var task = await _unitOfWork.TaskItemRepository.GetByIdAsync(id);
                if(task is null)
                    return Result<TaskDTO>.NotFound("the task does not exist");
                var toDoList = await _unitOfWork.ToDoListRepository.GetByIdAsync(updateTaskDTO.ToDoListId);

                if(toDoList is null)
                    return Result<TaskDTO>.NotFound("To Do List does not exist");

                task.Title = updateTaskDTO.Title;
                task.Description = updateTaskDTO.Description;
                task.IsDone = updateTaskDTO.IsDone;
                task.ToDoListId = updateTaskDTO.ToDoListId;
                _unitOfWork.TaskItemRepository.Update(task);
                await _unitOfWork.SaveChangesAsync();

                TaskDTO taskDTO = new()
                {
                    Title = updateTaskDTO.Title,
                    Description = updateTaskDTO.Description,
                    IsDone = updateTaskDTO.IsDone,
                    ToDoListId = updateTaskDTO.ToDoListId,
                    ToDoListName = toDoList.Name
                };
                return Result<TaskDTO>.Success(taskDTO);
            }
            catch(Exception ex)
            {
                return Result<TaskDTO>.Failure($"an error occurred while updating the task, {ex}");
            }
        }
        #endregion

        #region Delete
        public async Task<Result<TaskDTO>> DeleteAsync(int id)
        {
            try
            {
                var task = await _unitOfWork.TaskItemRepository.GetByIdAsync(id);
                if(task is null)
                    return Result<TaskDTO>.NotFound("the task does not exist");
                _unitOfWork.TaskItemRepository.Delete(task);
                await _unitOfWork.SaveChangesAsync();
                return Result<TaskDTO>.Success();
            }
            catch(Exception ex)
            {
                return Result<TaskDTO>.Failure($"an error occurred while deleting the task, {ex}");
            }
        }
        #endregion

        #region Mark As Done
        public async Task<Result<TaskDTO>> MarkAsDoneAsync(int id)
        {
            try
            {
                var task = await _unitOfWork.TaskItemRepository.GetByIdAsync(id);
                if(task is null)
                    return Result<TaskDTO>.NotFound("the task does not exist");
                task.IsDone = true;
                _unitOfWork.TaskItemRepository.Update(task);
                await _unitOfWork.SaveChangesAsync();
                return Result<TaskDTO>.Success();
            }
            catch(Exception ex)
            {
                return Result<TaskDTO>.Failure($"an error occurred while marking the task as done, {ex}");
            }
        }
        #endregion
    }
}
