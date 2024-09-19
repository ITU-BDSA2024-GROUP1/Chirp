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
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        var webService = new WebService(new CSVDatabase<Cheep>("data/chirp_cli_db.csv"));
        webService.Run();
    }

    public void Run(int port = 5000) => _app.Run($"http://localhost:{port}");

    public ValueTask DisposeAsync() => _app.DisposeAsync();
}