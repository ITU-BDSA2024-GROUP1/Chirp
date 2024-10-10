using Chirp.Core.Entities;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize);
    public Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public async Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize)
    {
        var cheepsResult = await _cheepRepository.GetAllCheepsAsync(page, pageSize);
        return new PagedResult<CheepViewModel>
        {
            Items = cheepsResult.Items.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList(),
            CurrentPage = cheepsResult.CurrentPage,
            TotalPages = cheepsResult.TotalPages
        };
    }

    public async Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize)
    {
        var cheepsResult = await _cheepRepository.GetCheepsByAuthorNameAsync(author, page, pageSize);
        return new PagedResult<CheepViewModel>
        {
            Items = cheepsResult.Items.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList(),
            CurrentPage = cheepsResult.CurrentPage,
            TotalPages = cheepsResult.TotalPages
        };
    }

}
