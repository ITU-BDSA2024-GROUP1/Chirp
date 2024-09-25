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
    }

    public static void Main(string[] args)
    {
        string path = "chirp_cli_db.csv";
#if DEBUG
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        path = "data/chirp_cli_db.csv";
        //var webService = new WebService(new CSVDatabase<Cheep>(Path.Combine(Directory.GetCurrentDirectory(), "chirp_cli_db.csv")));
#endif
        var webService = new WebService(new CSVDatabase<Cheep>(path));
        webService.Run();
    }

    public void Run(int port = 8080)
    {
        //var port = Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? "80"; // Default to 80 if not set
        _app.Run($"http://*:{port}");
    }

    public ValueTask DisposeAsync() => _app.DisposeAsync();
}
