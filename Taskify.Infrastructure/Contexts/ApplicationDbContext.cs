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

            // Global query filter for soft deletes
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

        #region SaveChanges Overriding
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var now = DateTime.UtcNow;
            foreach(var entry in ChangeTracker.Entries())
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
                if(entry.Entity is IDeletable deletableBaseEntity && entry.State==EntityState.Deleted)
                {
                    deletableBaseEntity.DeletedAt = now;
                    deletableBaseEntity.DeletedBy = userId;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var userId = GetCurrentUserId();
            var now = DateTime.UtcNow;
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
                if (entry.Entity is IDeletable deletableBaseEntity && entry.State == EntityState.Deleted)
                {
                    deletableBaseEntity.DeletedAt = now;
                    deletableBaseEntity.DeletedBy = userId;
                }
            }
            return base.SaveChanges();
        }
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
        #endregion

    }
}
