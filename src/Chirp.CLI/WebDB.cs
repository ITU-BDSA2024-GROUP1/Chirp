using System.Net.Http.Json;

using SimpleDB;

namespace Chirp.CLI;

public class WebDB<T> : IDatabaseRepository<T>
{
    private const string BaseUrl = "http://localhost:5127";
    private readonly HttpClient _client;
    
    public bool TestingDatabase = false;
    
    private static WebDB<T>? s_instance;
    private WebDB()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(BaseUrl);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new("application/json"));
    }
    public static WebDB<T> Instance
    {
        get => s_instance ??= new WebDB<T>();
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        string endpoint = (TestingDatabase ? "test/" : "") + "cheeps";
        if (limit != null) endpoint += $"?limit={limit}";

        var cheeps = _client.GetFromJsonAsync<IEnumerable<T>>(endpoint);
        return cheeps.Result!;
    }

    public void Store(T record)
    {
        string endpoint = (TestingDatabase ? "test/" : "") + "cheep";
        _client.PostAsJsonAsync(endpoint, record).Wait();
    }
}