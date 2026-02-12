using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Entities;
using Taskify.Domain.Repositories;
using Taskify.Infrastructure.Contexts;

namespace Taskify.Infrastructure.Repositories
{
    public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IDisposable
    {
        private readonly Lazy<IBaseRepository<TaskItem, int>> _taskItemRepository
            = new(() => new BaseRepository<TaskItem, int>(context));

        private readonly Lazy<IBaseRepository<Team, int>> _teamRepository
            = new(() => new BaseRepository<Team, int>(context));

        private readonly Lazy<IBaseRepository<ToDoList, int>> _toDoListRepository
            = new(() => new BaseRepository<ToDoList, int>(context));

        private readonly Lazy<IBaseRepository<UserTeam, int>> _userTeamRepository
            = new(() => new BaseRepository<UserTeam, int>(context));

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            return await context.Database.BeginTransactionAsync(ct);
        }
        private bool _disposed;
        public IBaseRepository<TaskItem, int> TaskItemRepository => _taskItemRepository.Value;
        public IBaseRepository<Team, int> TeamRepository => _teamRepository.Value;
        public IBaseRepository<ToDoList, int> ToDoListRepository => _toDoListRepository.Value;
        public IBaseRepository<UserTeam, int> UserTeamRepository => _userTeamRepository.Value;
        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                context.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async Task<int> SaveChangesAsync()
        {
            var count = await context.SaveChangesAsync();
            return count;
        }
    }
}
