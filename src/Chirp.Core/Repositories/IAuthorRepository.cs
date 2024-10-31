using Chirp.Core.DataTransferObject;

namespace Chirp.Core.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync();
    Task<AuthorDTO> GetAuthorByIdAsync(int id);
    Task<AuthorDTO> GetAuthorByNameAsync(string name);
    Task<AuthorDTO> GetAuthorByEmailAsync(string email);
    Task<int> AddAuthorAsync(AuthorDTO authorDto);
    Task UpdateAuthorAsync(AuthorDTO authorDto);
    Task<AuthorDTO> DeleteAuthorAsync(int id);
}