using Health_Care_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection 
        AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<HealthCareContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}