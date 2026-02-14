using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Const;
using Taskify.Domain.Entities;

namespace Taskify.Infrastructure
{
    public class ApplicationDbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                #region Seed Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                if (!roleManager.RoleExistsAsync(UserRole.User).Result)
                {
                    var role = new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRole.User
                    };
                    await roleManager.CreateAsync(role);
                }
                if (!roleManager.RoleExistsAsync(UserRole.Moderator).Result)
                {
                    var role = new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRole.Moderator
                    };
                    await roleManager.CreateAsync(role);
                }
                if (!roleManager.RoleExistsAsync(UserRole.Admin).Result)
                {
                    var role = new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        Name = UserRole.Admin
                    };
                    await roleManager.CreateAsync(role);
                }
                #endregion

                #region Seed Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                #region Seed Admin User
                var adminUser = userManager.FindByNameAsync("admin").Result;
                if (adminUser == null)
                {
                    var newAdmin = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "admin",
                        FullName = "Admin User",
                        Email = "admin@admin.com",
                        MobileNumber = "123456789"
                    };
                    var result = await userManager.CreateAsync(newAdmin, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin, UserRole.Admin);
                    }
                }
                #endregion

                #region Seed Moderator User
                var moderator = userManager.FindByNameAsync("moderator").Result;
                if (moderator == null)
                {
                    var newModerator = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "moderator",
                        FullName = "Moderator User",
                        Email = "moderator@moderator.com",
                        MobileNumber = "123456789"
                    };
                    var result = await userManager.CreateAsync(newModerator, "Moderator@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newModerator, UserRole.Moderator);
                    }
                }
                #endregion

                #region Seed user User
                var user = userManager.FindByNameAsync("user").Result;
                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "user",
                        FullName = "System User",
                        Email = "user@user.com",
                        MobileNumber = "123456789"
                    };
                    var result = await userManager.CreateAsync(newUser, "User@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, UserRole.User);
                    }
                }
                #endregion
                #endregion
            }
        }
    }
}