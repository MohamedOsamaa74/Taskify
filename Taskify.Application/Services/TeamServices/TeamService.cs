using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Taskify.Application.DTOs.TeamDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.AccountServices;
using Taskify.Domain.Entities;
using Taskify.Domain.Repositories;

namespace Taskify.Application.Services.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        public TeamService(IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }
        #region Create
        public async Task<Result<TeamDTO>>CreateAsync(CreateTeamDTO createTeamDTO)
        {
            try
            {
                var team = new Team
                {
                    Name = createTeamDTO.Name,
                    Description = createTeamDTO.Description
                };
                await _unitOfWork.TeamRepository.AddAsync(team);
                await _unitOfWork.SaveChangesAsync();
                var teamDTO = new TeamDTO
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description
                };
                return Result<TeamDTO>.Success(teamDTO);
            }
            catch (Exception ex)
            {
                return Result<TeamDTO>.Failure($"An error occurred while creating the team: {ex.Message}");
            }
        }
        #endregion

        #region GetById
        public async Task<Result<TeamDTO>> GetByIdAsync(int id)
        {
            try
            {
                var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);
                if(team is null)
                {
                    return Result<TeamDTO>.NotFound($"Team with id {id} not found.");
                }
                var teamDTO = new TeamDTO
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description
                };
                return Result<TeamDTO>.Success(teamDTO);
            }
            catch (Exception ex)
            {
                return Result<TeamDTO>.Failure($"An error occurred while retrieving the team: {ex.Message}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Result<List<TeamDTO>>> GetAllAsync()
        {
            try
            {
                var teams = _unitOfWork.TeamRepository.GetAll();
                var teamDTOs = await teams.Select(team => new TeamDTO
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description
                }).ToListAsync();
                return Result<List<TeamDTO>>.Success(teamDTOs);
            }
            catch (Exception ex)
            {
                return Result<List<TeamDTO>>.Failure($"An error occurred while retrieving teams: {ex.Message}");
            }
        }
        #endregion

        #region Update
        public async Task<Result<TeamDTO>> UpdateAsync(int id, CreateTeamDTO updateTeamDTO)
        {
            try
            {
                var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);
                if(team is null)
                {
                    return Result<TeamDTO>.NotFound($"Team with id {id} not found.");
                }
                team.Name = updateTeamDTO.Name;
                team.Description = updateTeamDTO.Description;
                _unitOfWork.TeamRepository.Update(team);
                await _unitOfWork.SaveChangesAsync();
                var teamDTO = new TeamDTO
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description
                };
                return Result<TeamDTO>.Updated(teamDTO);
            }
            catch (Exception ex)
            {
                return Result<TeamDTO>.Failure($"An error occurred while updating the team: {ex.Message}");
            }
        }
        #endregion

        #region Delete
        public async Task<Result<TeamDTO>> DeleteAsync(int id)
        {
            try
            {
                var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);
                if(team is null)
                {
                    return Result<TeamDTO>.NotFound($"Team with id {id} not found.");
                }
                _unitOfWork.TeamRepository.Delete(team);
                await _unitOfWork.SaveChangesAsync();
                return Result<TeamDTO>.Deleted();
            }
            catch (Exception ex)
            {
                return Result<TeamDTO>.Failure($"An error occurred while deleting the team: {ex.Message}");
            }
        }
        #endregion
    }
}
