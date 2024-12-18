using Chirp.Core.DataTransferObject;

namespace Chirp.Infrastructure.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync();
    Task<AuthorDTO> GetAuthorByIdAsync(string id);
    Task<AuthorDTO> GetAuthorByNameAsync(string name);
    Task<AuthorDTO> GetAuthorByEmailAsync(string email);
    Task<string> AddAuthorAsync(AuthorDTO authorDto);
    Task UpdateAuthorAsync(AuthorDTO authorDto);
    Task<AuthorDTO> DeleteAuthorAsync(string id);
}
