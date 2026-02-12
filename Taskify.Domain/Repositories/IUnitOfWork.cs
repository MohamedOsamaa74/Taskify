using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Entities;

namespace Taskify.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        IBaseRepository<TaskItem, int> TaskItemRepository { get;}
        IBaseRepository<Team, int> TeamRepository { get; }
        IBaseRepository<ToDoList, int> ToDoListRepository { get; }
        IBaseRepository<UserTeam, int> UserTeamRepository { get; }
    }
}
