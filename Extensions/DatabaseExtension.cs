using Microsoft.EntityFrameworkCore;

using AzureAPI.Database;

namespace AzureAPI.Extensions;

internal static class DatabaseExtension
{
    internal static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connection = "";

        if (environment.IsDevelopment())
        {
            connection = configuration.GetConnectionString("LOCAL_SQL_CONNECTIONSTRING");
            services.AddDbContext<AzureDbContext>(options => options.UseSqlServer(connection));
        }

        else if (environment.IsStaging())
        {
            connection = configuration.GetConnectionString("STAGING_SQL_CONNECTIONSTRING");
            services.AddDbContext<AzureDbContext>(options => options.UseSqlServer(connection));
        }

        else
        {
            connection = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
            services.AddDbContext<AzureDbContext>(options => options.UseAzureSql(connection));
        }

        Console.WriteLine($"Added by developer, this environment is: {configuration["Environment"]}");

        return services;
    }
}