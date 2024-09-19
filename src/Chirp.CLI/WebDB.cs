using System.Net.Http.Json;

using Chirp.Core;

using SimpleDB;

namespace Chirp.CLI;

public static class WebDB
{
    private const string BaseUrl = "http://localhost:5127";
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri(BaseUrl),
        DefaultRequestHeaders =
        {
            Accept =
            {
                new("application/json")
            }
        }
    };
    
    public static IEnumerable<Cheep> ReadCheeps(int? limit = null)
    {
        string endpoint = "cheeps" + (limit == null ? string.Empty : $"?limit={limit}");
        var cheeps = Client.GetFromJsonAsync<IEnumerable<Cheep>>(endpoint);
        return cheeps.Result!;
    }

    public static void WriteCheep(string message)
    {
        string endpoint = $"cheep?message={message}";
        Client.PostAsJsonAsync(endpoint, "how does this work").Wait();
    }
}