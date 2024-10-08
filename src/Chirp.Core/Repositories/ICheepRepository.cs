using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

namespace Chirp.Core.Repositories
{
    public interface ICheepRepository
    {
        Task<IEnumerable<CheepDTO>> GetAllCheepsAsync();
        Task<IEnumerable<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName);
        Task<int> AddCheepAsync(CheepDTO cheepDto);
        Task UpdateCheepAsync(CheepDTO cheepDto);
        Task<CheepDTO> DeleteCheepAsync(int id);
    }
}
