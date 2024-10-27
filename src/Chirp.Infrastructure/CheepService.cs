using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;
using Chirp.Core.Models;
using Chirp.Core.Repositories;
using Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);
public interface ICheepService
{
    public Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize);
    public Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize);
    public Task<CheepViewModel> GetCheepById(int id);
    public Task<int> PostCheep(CheepViewModel cheep);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
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

    public async Task<CheepViewModel> GetCheepById(int id)
    {
        return CheepDTOToCheepViewModel(await _cheepRepository.GetCheepByIdAsync(id));
    }

    public async Task<int> PostCheep(CheepViewModel cheepViewModel)
    {
        CheepDTO cheep = CheepViewModelToCheepDTO(cheepViewModel);
        AuthorDTO author = await _authorRepository.GetAuthorByNameAsync(cheep.Name);
        cheep.AuthorEmail = author.Email;
        cheep.AuthorId = author.Id;
        return await _cheepRepository.AddCheepAsync(cheep);
    }

    public static CheepViewModel CheepDTOToCheepViewModel(CheepDTO cheepDTO)
    {
        return new CheepViewModel(cheepDTO.Name, cheepDTO.Message, cheepDTO.TimeStamp);
    }

    public static CheepDTO CheepViewModelToCheepDTO(CheepViewModel cheepViewModel)
    {
        return new CheepDTO
        {
            Id = 0,
            Name = cheepViewModel.Author,
            Message = cheepViewModel.Message,
            TimeStamp = cheepViewModel.Timestamp,
            AuthorEmail = "",
            AuthorId = 0
        };
    }
}
