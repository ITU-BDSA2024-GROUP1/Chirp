using Chirp.Core.Data;

using Microsoft.EntityFrameworkCore;
using Chirp.Core.Repositories;

public class Program
{
    public static void Main(string[] args)
    {
        if (Environment.GetEnvironmentVariable("RUNNING_TESTS") == null)
        {
            Environment.SetEnvironmentVariable("RUNNING_TESTS", "false");
        }
        var builder = WebApplication.CreateBuilder(args);

        // Load database connection via configuration
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (Environment.GetEnvironmentVariable("RUNNING_TESTS").Equals("true"))
        {
            connectionString = builder.Configuration.GetConnectionString("TestingConnection");
        }
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
}
