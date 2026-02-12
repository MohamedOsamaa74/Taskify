using Microsoft.AspNetCore.Identity;
using Taskify.Domain.Entities;
using Taskify.Infrastructure.Contexts;

namespace Taskify.Api.Extensions
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
