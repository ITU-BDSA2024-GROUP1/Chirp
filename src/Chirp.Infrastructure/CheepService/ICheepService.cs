using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;

namespace Chirp.Infrastructure.CheepService;

public interface ICheepService
{
    public Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize);
    public Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize);
    public Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(List<string> authors, int page, int pageSize);
    public Task<CheepViewModel> GetCheepById(int id);
    public Task<int> PostCheep(CheepViewModel cheep);
    public Task<int> GetCheepCount(string authorName);
    public Task<CheepDTO> DeleteCheep(CheepViewModel cheep);
    
    public Task UpdateCheep(CheepViewModel cheep, string originalCheepMessage);
}