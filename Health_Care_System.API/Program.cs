using Health_Care_System.API.Extensions;
using Health_Care_System.Infrastructure.Persistence;
using Health_Care_System.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();

        builder.Services.AddSwaggerServices();
        builder.Services.AddDatabaseServices(builder.Configuration);
        builder.Services.AddRepositoryServices();
        builder.Services.AddMappingServices();


        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        await app.MigrateAndSeedDatabaseAsync();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        
        await app.RunAsync();
    }
}