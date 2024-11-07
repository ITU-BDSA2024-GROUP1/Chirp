using Chirp.Core;
using Chirp.Core.Data;
using Chirp.Core.Repositories;
using Chirp.Infrastructure.CheepService;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Chirp.Razor;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load database connection via configuration
        string? connectionString = GetConnectionString(builder);
        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => {
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<ChirpDBContext>();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout as needed
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true; // Make the session cookie essential
        });

        builder.Services.AddAuthentication()//options =>
        /*{
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "GitHub";
        })
        .AddCookie()*/
        .AddGitHub(o =>
        {
            o.ClientId = builder.Configuration["auth_github_clientId"];
            o.ClientSecret = builder.Configuration["auth_github_clientSecret"];
            o.CallbackPath = "/signin-github";
            o.Scope.Add("user:email");
            o.Scope.Add("read:user");
        });

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

        app.UseSession();

        app.MapRazorPages();

        app.Run();
    }

    private static string? GetConnectionString(WebApplicationBuilder builder)
    {
        string connectionString = TestEnvironmentManager.IsTestEnvironment() ? "TestingConnection" : "DefaultConnection";
        return builder.Configuration.GetConnectionString(connectionString);
    }
}