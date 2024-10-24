﻿using Chirp.Core.DataTransferObject;
using Chirp.Core.Models;

namespace Chirp.Core.Repositories
{
    public interface ICheepRepository
    {
        Task<PagedResult<CheepDTO>> GetAllCheepsAsync(int page, int pageSize);
        Task<PagedResult<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName, int page, int pageSize);
        Task<int> AddCheepAsync(CheepDTO cheepDto);
        Task UpdateCheepAsync(CheepDTO cheepDto);
        Task<CheepDTO?> DeleteCheepAsync(int id);
    }
}
