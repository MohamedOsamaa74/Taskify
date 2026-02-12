using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Entities;

namespace Taskify.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<ApplicationRole> roles { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }

    }
}
