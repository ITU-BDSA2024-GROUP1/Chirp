using Chirp.Core;
using Chirp.Core.Data;
using Chirp.Core.Repositories;
using Chirp.Infrastructure.CheepService;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Razor;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load database connection via configuration
        string? connectionString = GetConnectionString(builder);
        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ChirpDBContext>();

        // Register the repositories
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<ICheepService, CheepService>();

        var app = builder.Build();

        // Seed the database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ChirpDBContext>();
            await context.Database.EnsureCreatedAsync();
            DbInitializer.SeedDatabase(context);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }

    private static string? GetConnectionString(WebApplicationBuilder builder)
    {
        string connectionString = IsTestEnvironment() ? "TestingConnection" : "DefaultConnection";
        return builder.Configuration.GetConnectionString(connectionString);
    }
    
    private static bool IsTestEnvironment()
    {
        const string testEnvVar = "RUNNING_TESTS";
        
        string? environment = GetTestEnvironmentVariable();
        switch (environment)
        {
            case null:
                SetTestEnvironmentVariable("false");
                return false;
            case "true":
                return true;
            case "false":
                return false;
            default:
                throw new ArgumentException(
                    $"{testEnvVar} environment variable, was neither true nor false. (Actual: {environment})");
        }
        
        string? GetTestEnvironmentVariable() => Environment.GetEnvironmentVariable(testEnvVar);
        void SetTestEnvironmentVariable(string value) => Environment.SetEnvironmentVariable(testEnvVar, value);
    }
}