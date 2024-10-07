using Chirp.Razor;

using Microsoft.Data.Sqlite;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");

        // Initialize the database if it doesn't exist
        if (!File.Exists(dbPath))
        {
            InitializeDatabase(dbPath);
        }

        builder.Services.AddSingleton(new DBFacade(dbPath));

        builder.Services.AddSingleton<ICheepService, CheepService>();


        var app = builder.Build();

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

        static void InitializeDatabase(string dbPath)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath};");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        create table if not exists user (
          user_id integer primary key autoincrement,
          username string not null,
          email string not null,
          pw_hash string not null
        );

        create table if not exists message (
          message_id integer primary key autoincrement,
          author_id integer not null,
          text string not null,
          pub_date integer
        );
    ";
            command.ExecuteNonQuery();
        }
    }
}