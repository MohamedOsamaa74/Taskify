using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.ToDoListDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Domain.Entities;
using Taskify.Domain.Repositories;

namespace Taskify.Application.Services.ToDoListServices
{
    public class ToDoListService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : IToDoListService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManagser = userManager;

        #region Create
        public async Task<Result<ToDoListDTO>> CreateAsync(CreateToDoListDTO createToDoListDTO)
        {
            try
            {
                if (createToDoListDTO.ApplicationUserId is null && createToDoListDTO.TeamId is null)
                {
                    return Result<ToDoListDTO>.Failure("ApplicationUserId or TeamId must be provided");
                }

                if (createToDoListDTO.ApplicationUserId is not null && createToDoListDTO.TeamId is not null)
                {
                    return Result<ToDoListDTO>.Failure("The ToDo List cannot belong to both user and team, only one");
                }

                ApplicationUser? user = null;
                if (createToDoListDTO.ApplicationUserId is not null)
                {
                    user = await _userManagser.FindByIdAsync(createToDoListDTO.ApplicationUserId.Value.ToString());
                    if (user is null)
                    {
                        return Result<ToDoListDTO>.Failure("User Does not exist");
                    }
                }

                Team? team = null;
                if (createToDoListDTO.TeamId is not null)
                {
                    team = await _unitOfWork.TeamRepository.GetByIdAsync(createToDoListDTO.TeamId.Value);
                    if (team is null)
                    {
                        return Result<ToDoListDTO>.Failure("Team Does not exist");
                    }
                }

                var toDoList = new ToDoList
                {
                    Name = createToDoListDTO.Name,
                    Description = createToDoListDTO.Description,
                    TeamId = createToDoListDTO.TeamId,
                    ApplicationUserId = createToDoListDTO.ApplicationUserId
                };
                await _unitOfWork.ToDoListRepository.AddAsync(toDoList);
                await _unitOfWork.SaveChangesAsync();

                var toDoListDTO = new ToDoListDTO
                {
                    Id = toDoList.Id,
                    Name = toDoList.Name,
                    Description = toDoList.Description,
                    TeamId = toDoList?.TeamId,
                    TeamName = team?.Name,
                    ApplicationUserId = toDoList?.ApplicationUserId,
                    UserName = user?.UserName
                };
                return Result<ToDoListDTO>.Success(toDoListDTO);
            }
            catch (Exception ex)
            {
                return Result<ToDoListDTO>.Failure($"an error occurred while creating the list. {ex}");
            }
        }
        #endregion

        #region Get By Id
        public async Task<Result<ToDoListDTO>> GetByIdAsync(int id)
        {
            try
            {
                var toDoList = await _unitOfWork.ToDoListRepository.GetByIdAsync(id);
                if(toDoList is null)
                {
                    return Result<ToDoListDTO>.Failure("List Does not exist");
                }
                Team? team = null;
                if (toDoList.TeamId.HasValue)
                {
                    team = await _unitOfWork.TeamRepository.GetByIdAsync(toDoList.TeamId.Value);
                }

                ApplicationUser? user = null;
                if (toDoList.ApplicationUserId.HasValue)
                {
                    user = await _userManagser.FindByIdAsync(toDoList.ApplicationUserId.Value.ToString());
                }

                var toDoListDTO = new ToDoListDTO
                {
                    Id = toDoList.Id,
                    Name = toDoList.Name,
                    Description = toDoList.Description,
                    TeamId = toDoList?.TeamId,
                    TeamName = team?.Name,
                    ApplicationUserId = toDoList?.ApplicationUserId,
                    UserName = user?.UserName
                };
                return Result<ToDoListDTO>.Success(toDoListDTO);
            }
            catch(Exception ex)
            {
                return Result<ToDoListDTO>.Failure($"an error occurred while fetching the list. {ex}");
            }
        }
        #endregion

        #region Get User Lists
        public async Task<Result<List<ToDoListDTO>>> GetUserToDoListsAsync(Guid userId)
        {
            try
            {
                var user = await _userManagser.FindByIdAsync(userId.ToString());
                if(user is null)
                {
                    return Result<List<ToDoListDTO>>.Failure("user does not exist");
                }
                var toDoLists = await _unitOfWork.ToDoListRepository
                    .GetAll(t => t.ApplicationUserId == userId)
                    .Include(td => td.ApplicationUser)
                    .Select(tdt => new ToDoListDTO
                    {
                        Id = tdt.Id,
                        Name = tdt.Name,
                        Description = tdt.Description,
                        ApplicationUserId = tdt.ApplicationUserId,
                        UserName = user.UserName
                    }).ToListAsync();
                return Result<List<ToDoListDTO>>.Success(toDoLists);
            }
            catch(Exception ex)
            {
                return Result<List<ToDoListDTO>>.Failure($"an error occurred while fetching the User lists. {ex}");
            }
        }
        #endregion

        #region Get Team Lists
        public async Task<Result<List<ToDoListDTO>>> GetTeamToDoListsAsync(int teamId)
        {
            try
            {
                var team = await _unitOfWork.TeamRepository.GetByIdAsync(teamId);
                if (team is null)
                {
                    return Result<List<ToDoListDTO>>.Failure("team does not exist");
                }
                var toDoLists = await _unitOfWork.ToDoListRepository
                    .GetAll(t => t.TeamId == teamId)
                    .Include(td => td.Team)
                    .Select(tdt => new ToDoListDTO
                    {
                        Id = tdt.Id,
                        Name = tdt.Name,
                        Description = tdt.Description,
                        TeamId = teamId,
                        TeamName = team.Name
                    }).ToListAsync();
                return Result<List<ToDoListDTO>>.Success(toDoLists);
            }
            catch (Exception ex)
            {
                return Result<List<ToDoListDTO>>.Failure($"an error occurred while fetching Team lists. {ex}");
            }
        }
        #endregion
    }
}
