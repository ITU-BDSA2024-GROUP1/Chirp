using System.Net.Http.Headers;
using System.Net.Http.Json;

using SimpleDB;

namespace Chirp.CLI;

public class WebDB<T> : IDatabaseRepository<T>
{
    private readonly HttpClient _client;
    
    public WebDB(string baseUrl)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(baseUrl);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        string endpoint = limit == null ? "cheeps" : $"cheeps?limit={limit}";

        var cheeps = _client.GetFromJsonAsync<IEnumerable<T>>(endpoint);
        return cheeps.Result!;
    }

    public void Store(T record) => _client.PostAsJsonAsync("cheep", record).Wait();
}