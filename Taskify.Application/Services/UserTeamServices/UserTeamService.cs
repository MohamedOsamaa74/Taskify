using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Application.DTOs.UserTeamDTOs;
using Taskify.Application.ResultPattern;
using Taskify.Application.Services.AccountServices;
using Taskify.Domain.Entities;
using Taskify.Domain.Repositories;

namespace Taskify.Application.Services.UserTeamServices
{
    public class UserTeamService : IUserTeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserTeamService(IUnitOfWork unitOfWork, IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _userManager = userManager;
        }

        #region Add user to team
        public async Task<Result<UserTeamDTO>> AddUserToTeamAsync(UserTeamDTO userTeamDTO)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userTeamDTO.UserId);
                if (user == null)
                {
                    return Result<UserTeamDTO>.Failure("User does not exist");
                }
                var team = await _unitOfWork.TeamRepository.GetByIdAsync(userTeamDTO.TeamId);
                if (team == null)
                {
                    return Result<UserTeamDTO>.Failure("Team does not exist");
                }
                var userTeam = new UserTeam
                {
                    UserId = userTeamDTO.UserId,
                    TeamId = userTeamDTO.TeamId
                };
                await _unitOfWork.UserTeamRepository.AddAsync(userTeam);
                await _unitOfWork.SaveChangesAsync();
                return Result<UserTeamDTO>.Success();
            }
            catch (Exception ex)
            {
                return Result<UserTeamDTO>.Failure($"an error occured, {ex}");
            }
        }
        #endregion

        #region Remove user from team
        public async Task<Result<UserTeamDTO>> RemoveUserFromTeamAsync(UserTeamDTO userTeamDTO)
        {
            try
            {
                var userTeam = await _unitOfWork.UserTeamRepository
                    .GetSingleAsync(u => u.UserId== userTeamDTO.UserId && u.TeamId == userTeamDTO.TeamId);
                if (userTeam == null)
                {
                    return Result<UserTeamDTO>.Failure($"Invalid input");
                }
                _unitOfWork.UserTeamRepository.Delete(userTeam);
                await _unitOfWork.SaveChangesAsync();
                return Result<UserTeamDTO>.Success();
            }
            catch (Exception ex)
            {
                return Result<UserTeamDTO>.Failure($"an error occured, {ex}");
            }
        }
        #endregion

        #region Get User Teams
        public async Task<Result<List<UserTeamDTO>>> GetUserTeamsAsync(Guid? userId)
        {
            try
            {
                userId ??= _accountService.GetCurrentUserId().Result.Data;
                var userTeams = await _unitOfWork.UserTeamRepository
                    .GetAll(u => u.UserId == userId)
                    .Select(ut => new UserTeamDTO {UserId = ut.UserId, TeamId = ut.TeamId })
                    .ToListAsync();
                if(userTeams is null)
                    return Result<List<UserTeamDTO>>.Failure($"Couldn't fetch the user Teams");
                return Result<List<UserTeamDTO>>.Success(userTeams);
            }
            catch(Exception ex) {
                return Result<List<UserTeamDTO>>.Failure($"an error occured, {ex}");
            }
        }
        #endregion

    }
}
