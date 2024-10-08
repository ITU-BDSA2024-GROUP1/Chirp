using Chirp.Core.Repositories;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepViewModel>> GetCheeps();
    public Task<List<CheepViewModel>> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public async Task<List<CheepViewModel>> GetCheeps()
    {
        var cheeps = await _cheepRepository.GetAllCheepsAsync();
        return cheeps.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList();
    }

    public async Task<List<CheepViewModel>> GetCheepsFromAuthor(string author)
    {
        var cheeps = await _cheepRepository.GetCheepsByAuthorNameAsync(author);
        return cheeps.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList();
    }
}
