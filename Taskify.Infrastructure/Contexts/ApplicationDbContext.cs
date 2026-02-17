using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Taskify.Domain.Common.Interfaces;
using Taskify.Domain.Entities;

namespace Taskify.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<ApplicationRole>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<TaskItem>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Team>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<ToDoList>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<UserTeam>().HasQueryFilter(e => e.DeletedAt == null);
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<ApplicationRole> roles { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }

        #region Save Changes Overriding
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var now = DateTime.UtcNow;

            HandleSoftDelete(userId, now);

            foreach (var entry in ChangeTracker.Entries())
            {
                if(entry.Entity is IAuditable auditableBaseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditableBaseEntity.CreatedAt = now;
                            auditableBaseEntity.CreatedBy = userId;
                            break;
                        case EntityState.Modified:
                            auditableBaseEntity.UpdatedAt = now;
                            auditableBaseEntity.UpdatedBy = userId;
                            break;
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var userId = GetCurrentUserId();
            var now = DateTime.UtcNow;

            HandleSoftDelete(userId, now);

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable auditableBaseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditableBaseEntity.CreatedAt = now;
                            auditableBaseEntity.CreatedBy = userId;
                            break;
                        case EntityState.Modified:
                            auditableBaseEntity.UpdatedAt = now;
                            auditableBaseEntity.UpdatedBy = userId;
                            break;
                    }
                }
            }
            return base.SaveChanges();
        }

        private void HandleSoftDelete(Guid? userId, DateTime now)
        {
            // Get all entities marked for deletion
            var deletedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is IDeletable)
                .ToList();

            foreach (var entry in deletedEntries)
            {
                var deletableEntity = (IDeletable)entry.Entity;

                // Set soft delete properties
                deletableEntity.DeletedAt = now;
                deletableEntity.DeletedBy = userId;

                // Change state to Modified instead of Deleted
                entry.State = EntityState.Modified;

                // Handle cascading soft delete for specific entities
                CascadeSoftDelete(entry.Entity, userId, now);
            }
        }

        private void CascadeSoftDelete(object entity, Guid? userId, DateTime now)
        {
            switch (entity)
            {
                case ToDoList toDoList:
                    // Soft delete all TaskItems in this ToDoList
                    var taskItems = Entry(toDoList)
                        .Collection(t => t.Items)
                        .Query()
                        .Where(ti => ti.DeletedAt == null) // Only active items
                        .ToList();

                    foreach (var taskItem in taskItems)
                    {
                        taskItem.DeletedAt = now;
                        taskItem.DeletedBy = userId;
                        Entry(taskItem).State = EntityState.Modified;
                    }
                    break;

                case Team team:
                    // Soft delete all ToDoLists in this Team
                    var teamToDoLists = Entry(team)
                        .Collection(t => t.ToDoLists)
                        .Query()
                        .Where(tdl => tdl.DeletedAt == null)
                        .Include(tdl => tdl.Items) // Include items for cascading
                        .ToList();

                    foreach (var toDoList in teamToDoLists)
                    {
                        toDoList.DeletedAt = now;
                        toDoList.DeletedBy = userId;
                        Entry(toDoList).State = EntityState.Modified;

                        // Cascade to TaskItems
                        foreach (var taskItem in toDoList.Items.Where(ti => ti.DeletedAt == null))
                        {
                            taskItem.DeletedAt = now;
                            taskItem.DeletedBy = userId;
                            Entry(taskItem).State = EntityState.Modified;
                        }
                    }

                    // Soft delete all UserTeam relationships
                    var userTeams = Entry(team)
                        .Collection(t => t.UserTeams)
                        .Query()
                        .Where(ut => ut.DeletedAt == null)
                        .ToList();

                    foreach (var userTeam in userTeams)
                    {
                        userTeam.DeletedAt = now;
                        userTeam.DeletedBy = userId;
                        Entry(userTeam).State = EntityState.Modified;
                    }
                    break;

                case ApplicationUser user:
                    // Soft delete all user's ToDoLists
                    var userToDoLists = Entry(user)
                        .Collection(u => u.ToDoLists)
                        .Query()
                        .Where(tdl => tdl.DeletedAt == null)
                        .Include(tdl => tdl.Items)
                        .ToList();

                    foreach (var toDoList in userToDoLists)
                    {
                        toDoList.DeletedAt = now;
                        toDoList.DeletedBy = userId;
                        Entry(toDoList).State = EntityState.Modified;

                        // Cascade to TaskItems
                        foreach (var taskItem in toDoList.Items.Where(ti => ti.DeletedAt == null))
                        {
                            taskItem.DeletedAt = now;
                            taskItem.DeletedBy = userId;
                            Entry(taskItem).State = EntityState.Modified;
                        }
                    }

                    // Soft delete all UserTeam relationships
                    var userTeamRels = Entry(user)
                        .Collection(u => u.UserTeams)
                        .Query()
                        .Where(ut => ut.DeletedAt == null)
                        .ToList();

                    foreach (var userTeam in userTeamRels)
                    {
                        userTeam.DeletedAt = now;
                        userTeam.DeletedBy = userId;
                        Entry(userTeam).State = EntityState.Modified;
                    }
                    break;
            }
        }
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
        #endregion

    }
}
