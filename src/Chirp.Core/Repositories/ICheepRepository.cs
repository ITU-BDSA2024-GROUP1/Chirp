using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

namespace Chirp.Core.Repositories
{
    public interface ICheepRepository
    {
        Task<IEnumerable<CheepDTO>> GetAllCheepsAsync(int page);
        Task<IEnumerable<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName, int page);
        Task<int> AddCheepAsync(CheepDTO cheepDto);
        Task UpdateCheepAsync(CheepDTO cheepDto);
        Task<CheepDTO> DeleteCheepAsync(int id);
    }
}
