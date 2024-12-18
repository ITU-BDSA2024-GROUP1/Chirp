using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure.Models;

namespace Chirp.Infrastructure.Repositories;

public interface ICheepRepository
{
    Task<PagedResult<CheepDTO>> GetAllCheepsAsync(int page, int pageSize);
    Task<PagedResult<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName, int page, int pageSize);
    Task<PagedResult<CheepDTO>> GetCheepsByAuthorNameAsync(List<string> authorNames, int page, int pageSize);
    Task<CheepDTO> GetCheepByIdAsync(int id);
    Task<int> AddCheepAsync(CheepDTO cheepDto);
    Task UpdateCheepAsync(CheepDTO cheepDto, string originalCheepMessage);
    Task<CheepDTO> DeleteCheepAsync(int id);
    Task<int> GetCheepCountByAuthor(string authorName);
    Task<CheepDTO> GetCheepByNotIDAsync(string author, string message, string timestamp);
}
