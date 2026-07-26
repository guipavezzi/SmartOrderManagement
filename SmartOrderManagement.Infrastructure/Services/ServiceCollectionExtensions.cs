using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartOrderManagement.Infrastructure.Data.Context;

namespace SmartOrderManagement.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbDirectory = Path.Combine(appDataPath, "SmartOrderManagement");
        
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        var dbPath = Path.Combine(dbDirectory, "smartordermanagement.db");
        var connectionString = $"Data Source={dbPath}";

        services.AddDbContext<SmartOrderManagementDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}