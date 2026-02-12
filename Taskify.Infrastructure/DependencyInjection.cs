using Microsoft.Extensions.DependencyInjection;
using Taskify.Domain.Repositories;
using Taskify.Infrastructure.Repositories;

namespace Taskify.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
