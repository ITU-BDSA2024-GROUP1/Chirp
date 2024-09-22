using Chirp.Core;

using SimpleDB;

namespace Chirp.CSVDBService;

public class WebService : IAsyncDisposable
{
    private readonly WebApplication _app;

    public WebService(IDatabaseRepository<Cheep> repository)
    {
        var builder = WebApplication.CreateBuilder();
        _app = builder.Build();
        
        _app.MapGet("/cheeps", repository.Read);
        _app.MapPost("/cheep", repository.Store);
        _app.MapGet("/health", () => Results.Ok("Healthy")); // Health check endpoint
    }

    public static void Main(string[] args)
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var webService = new WebService(new CSVDatabase<Cheep>(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "chirp_cli_db.csv")));
        webService.Run();
    }

    public void Run()
    {
        //var port = Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? "80"; // Default to 80 if not set
        _app.Run($"http://0.0.0.0:8181");
    }

    public ValueTask DisposeAsync() => _app.DisposeAsync();
}