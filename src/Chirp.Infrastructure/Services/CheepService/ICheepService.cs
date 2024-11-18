using Chirp.Infrastructure.Models;

namespace Chirp.Infrastructure.Services.CheepService;

public interface ICheepService
{
    public Task<PagedResult<CheepViewModel>> GetCheeps(int page, int pageSize);
    public Task<PagedResult<CheepViewModel>> GetCheepsFromAuthor(string author, int page, int pageSize);
    public Task<CheepViewModel> GetCheepById(int id);
    public Task<int> PostCheep(CheepViewModel cheep);
}