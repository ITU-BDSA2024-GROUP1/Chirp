using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

namespace Chirp.Infrastructure.CheepService;

public class CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository) : ICheepService
{
    public async Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize)
    {
        var cheepsResult = await cheepRepository.GetAllCheepsAsync(page, pageSize);
        return new()
        {
            Items = cheepsResult.Items.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList(),
            CurrentPage = cheepsResult.CurrentPage,
            TotalPages = cheepsResult.TotalPages
        };
    }

    public async Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize)
    {
        var cheepsResult = await cheepRepository.GetCheepsByAuthorNameAsync(author, page, pageSize);
        return new()
        {
            Items = cheepsResult.Items.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList(),
            CurrentPage = cheepsResult.CurrentPage,
            TotalPages = cheepsResult.TotalPages
        };
    }

    public async Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(List<string> authors, int page, int pageSize)
    {
        var cheepsResult = await cheepRepository.GetCheepsByAuthorNameAsync(authors, page, pageSize);
        return new()
        {
            Items = cheepsResult.Items.Select(c => new CheepViewModel(c.Name, c.Message, c.TimeStamp)).ToList(),
            CurrentPage = cheepsResult.CurrentPage,
            TotalPages = cheepsResult.TotalPages
        };
    }

    public async Task<CheepViewModel> GetCheepById(int id)
    {
        return CheepDTOToCheepViewModel(await cheepRepository.GetCheepByIdAsync(id));
    }

    public async Task<int> PostCheep(CheepViewModel cheepViewModel)
    {
        CheepDTO cheep = CheepViewModelToCheepDTO(cheepViewModel);
        AuthorDTO author = await authorRepository.GetAuthorByNameAsync(cheep.Name);
        cheep.AuthorEmail = author.Email;
        cheep.AuthorId = author.Id;
        return await cheepRepository.AddCheepAsync(cheep);
    }

    public static CheepViewModel CheepDTOToCheepViewModel(CheepDTO cheepDTO)
    {
        return new(cheepDTO.Name, cheepDTO.Message, cheepDTO.TimeStamp);
    }

    private static CheepDTO CheepViewModelToCheepDTO(CheepViewModel cheepViewModel)
    {
        return new()
        {
            Id = 0,
            Name = cheepViewModel.Author,
            Message = cheepViewModel.Message,
            TimeStamp = cheepViewModel.Timestamp,
            AuthorEmail = "",
            AuthorId = "0"
        };
    }

    public Task<int> GetCheepCount(string authorName)
    {
        return cheepRepository.GetCheepCountByAuthor(authorName);
    }

    public async Task<CheepDTO> DeleteCheep(CheepViewModel cheep)
    {
        return await cheepRepository.DeleteCheepAsync((await cheepRepository.GetCheepByNotIDAsync(cheep.Author, cheep.Message, cheep.Timestamp)).Id);
    }
    public async Task UpdateCheep(CheepViewModel cheep)
    {
        Console.WriteLine("Updating cheep in service"+cheep.Author+" " +cheep.Timestamp+" "+cheep.Message);
        await cheepRepository.UpdateCheepAsync(CheepViewModelToCheepDTO(cheep));
    }
}