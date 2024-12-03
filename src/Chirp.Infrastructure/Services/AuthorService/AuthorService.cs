using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure.Repositories;

namespace Chirp.Infrastructure.Services.AuthorService;

public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
{
    public async Task<AuthorViewModel> GetAuthorByName(string author)
    {
        var authorResult = await authorRepository.GetAuthorByNameAsync(author);
        return AuthorDTOToAuthorViewModel(authorResult);
    }

    public async Task<AuthorViewModel> GetAuthorByEmail(string email)
    {
        var authorResult = await authorRepository.GetAuthorByEmailAsync(email);
        return AuthorDTOToAuthorViewModel(authorResult);
    }

    public async Task<AuthorViewModel> GetAuthorById(string id)
    {
        return AuthorDTOToAuthorViewModel(await authorRepository.GetAuthorByIdAsync(id));
    }
    
    public async Task<string> CreateAuthor(AuthorViewModel authorViewModel)
    {
        AuthorDTO author = AuthorViewModelToAuthorDTO(authorViewModel);
        return await authorRepository.AddAuthorAsync(author);
    }
    
    public async Task<AuthorDTO> DeleteAuthor(AuthorViewModel author)
    {
        return await authorRepository.DeleteAuthorAsync(author.Id);
    }

    private static AuthorViewModel AuthorDTOToAuthorViewModel(AuthorDTO authorDTO)
    {
        return new(authorDTO.Id, authorDTO.Name, authorDTO.Email);
    }

    private static AuthorDTO AuthorViewModelToAuthorDTO(AuthorViewModel authorViewModel)
    {
        return new()
        {
            Id = authorViewModel.Id,
            Name = authorViewModel.Name,
            Email = authorViewModel.Email
        };
    }
}