using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

namespace Chirp.Infrastructure;

public record AuthorViewModel(int Id, string Name, string Email);
public interface IAuthorService
{
    public Task<AuthorViewModel> GetAuthorByName(string author);
    public Task<AuthorViewModel> GetAuthorByEmail(string email);
    public Task<int> CreateAuthor(AuthorViewModel author);
}
public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorViewModel> GetAuthorByName(string author)
    {
        var authorResult = await _authorRepository.GetAuthorByNameAsync(author);
        return new AuthorViewModel(authorResult.Id, authorResult.Name, authorResult.Email);
    }

    public async Task<AuthorViewModel> GetAuthorByEmail(string email)
    {
        var authorResult = await _authorRepository.GetAuthorByEmailAsync(email);
        return new AuthorViewModel(authorResult.Id, authorResult.Name, authorResult.Email);
    }

    public async Task<int> CreateAuthor(AuthorViewModel authorViewModel)
    {
        AuthorDTO author = AuthorViewModelToAuthorDTO(authorViewModel);
        return await _authorRepository.AddAuthorAsync(author);
    }


    public static AuthorViewModel AuthorDTOToAuthorViewModel(AuthorDTO authorDTO)
    {
        return new AuthorViewModel(authorDTO.Id, authorDTO.Name, authorDTO.Email);
    }

    public static AuthorDTO AuthorViewModelToAuthorDTO(AuthorViewModel authorViewModel)
    {
        return new AuthorDTO
        {
            Id = authorViewModel.Id,
            Name = authorViewModel.Name,
            Email = authorViewModel.Email
        };
    }
}
