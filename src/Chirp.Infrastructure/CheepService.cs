using Chirp.Core.Entities;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

public record CheepViewModel(string Author, string Message, string Timestamp);
public record AuthorViewModel(int Id, string Name, string Email);
public interface ICheepService
{
    public Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize);
    public Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
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

    public async Task<AuthorViewModel> GetAuthorFromName(string author)
    {
        var authorResult = await _authorRepository.GetAuthorByNameAsync(author);
        return new AuthorViewModel(authorResult.Id, authorResult.Name, authorResult.Email);
    }
    
    public async Task<AuthorViewModel> GetAuthorFromEmail(string email)
    {
        var authorResult = await _authorRepository.GetAuthorByEmailAsync(email);
        return new AuthorViewModel(authorResult.Id, authorResult.Name, authorResult.Email);
    }
}
