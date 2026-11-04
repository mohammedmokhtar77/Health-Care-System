using System.Text;
using Health_Care_System.API.DTOs.Mapping;
using Health_Care_System.API.Services.Implementations;
using Health_Care_System.API.Services.Interfaces;
using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Repositories.Interfaces;
using Health_Care_System.Infrastructure.Persistence;
using Health_Care_System.Infrastructure.Persistence.Interceptors;
using Health_Care_System.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace Health_Care_System.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token. Swagger will add the Bearer prefix."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });   
        return services;
    }

    public static IServiceCollection 
        AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<HealthCareContext>(options =>
            options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(
                    new SoftDeleteInterceptor()
                    , new CreatedDateInterceptor() 
                    , new ModifiedDateInceptor()
                    )
        );
            

        return services;
    }

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
            services.AddDbContext<AuthenticationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AuthConnection"))
            );
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                }
            );
        
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