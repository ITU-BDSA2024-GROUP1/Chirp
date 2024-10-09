using Chirp.Core.Repositories;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepViewModel>> GetCheeps(int page);
    public Task<List<CheepViewModel>> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public async Task<List<CheepViewModel>> GetCheeps(int page)
    {
        var cheeps = await _cheepRepository.GetAllCheepsAsync(page);
        return cheeps.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList();
    }

    public async Task<List<CheepViewModel>> GetCheepsFromAuthor(string author, int page)
    {
        var cheeps = await _cheepRepository.GetCheepsByAuthorNameAsync(author, page);
        return cheeps.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList();
    }
}
