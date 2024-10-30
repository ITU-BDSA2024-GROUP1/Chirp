using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

namespace Chirp.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync();
        Task<AuthorDTO> GetAuthorByIdAsync(string id);
        Task<string> AddAuthorAsync(AuthorDTO authorDto);
        Task UpdateAuthorAsync(AuthorDTO authorDto);
        Task<AuthorDTO> DeleteAuthorAsync(string id);
    }
}
