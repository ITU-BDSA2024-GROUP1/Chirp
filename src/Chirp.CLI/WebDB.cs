using System.Net.Http.Headers;
using System.Net.Http.Json;

using SimpleDB;

namespace Chirp.CLI;

public class WebDB<T> : IDatabaseRepository<T>
{
    private readonly HttpClient _client;
    
    public WebDB(string baseUrl)
    {
        _client = new();
        _client.BaseAddress = new(baseUrl);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new("application/json"));
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        string endpoint = 
            limit == null ? 
            "cheeps" : 
            $"cheeps?limit={limit}";

        var response = _client.GetAsync(endpoint).Result;
        response.EnsureSuccessStatusCode();
        
        var cheeps = response.Content.ReadFromJsonAsync<IEnumerable<T>>().Result;
        return cheeps!;
    }

    public void Store(T record) => _client.PostAsJsonAsync("cheep", record).Wait();
}