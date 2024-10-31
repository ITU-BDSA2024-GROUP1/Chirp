using Chirp.Core.Data;
using Chirp.Core.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load database connection via configuration
        string? connectionString = GetConnectionString(builder);
        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

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
            context.Database.EnsureCreatedAsync();
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
        string? environment = GetTestEnvironmentVariable();
        if (environment == null)
        {
            SetTestEnvironmentVariable("false");
            return false;
        }

        return environment switch
        {
            "true" => true,
            "false" => false,
            _ => throw new ArgumentException($"RUNNING_TESTS environment variable, was neither true nor false. (Actual: {environment})")
        };
        
        const string testEnvVar = "RUNNING_TESTS";
        string? GetTestEnvironmentVariable() => Environment.GetEnvironmentVariable(testEnvVar);
        void SetTestEnvironmentVariable(string value) => Environment.SetEnvironmentVariable(testEnvVar, value);
    }
}