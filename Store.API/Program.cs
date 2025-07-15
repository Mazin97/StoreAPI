using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Service.Store;

namespace StoreAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Services
        builder.Services.AddScoped<IStoreService, StoreService>();
        builder.Services.AddScoped<IAuthorizationService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var utilsServiceUrl = config["UtilsServiceUrl"]
                ?? throw new InvalidOperationException("UtilsServiceUrl config missing");

            return new AuthorizationService(utilsServiceUrl);
        });
        builder.Services.AddScoped<INotificationService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var utilsServiceUrl = config["UtilsServiceUrl"]
                ?? throw new InvalidOperationException("UtilsServiceUrl config missing");

            if (int.TryParse(config["NotificationMaxRetries"], out int maxRetries) == false)
            {
                maxRetries = 3;
            }

            return new NotificationService(utilsServiceUrl, maxRetries);
        });

        // Repositories
        builder.Services.AddScoped<IStoreRepository, StoreRepository>();

        builder.Services.AddDbContext<StoreContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
