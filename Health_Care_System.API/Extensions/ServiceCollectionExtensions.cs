using Health_Care_System.API.DTOs.Mapping;
using Health_Care_System.API.Services.Implementations;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Repositories.Interfaces;
using Health_Care_System.Infrastructure.Persistence;
using Health_Care_System.Infrastructure.Persistence.Repositories;
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

    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDoctorService, DoctorServices>();
        services.AddScoped<IPatientService, PatientServices>();
        services.AddScoped<IAppointmentService, AppointmentServices>();
        return services;
    }

    public static IServiceCollection AddMappingServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PatientProfile>();
            cfg.AddProfile<DoctorProfile>();
            cfg.AddProfile<AppointmentProfile>();
        });
        return services;
    }
}