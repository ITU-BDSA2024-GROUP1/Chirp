using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;
using Chirp.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Repositories
{
    public class CheepRepository : ICheepRepository
    {
        private readonly ChirpDBContext _dbContext;
        public CheepRepository(ChirpDBContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<int> AddCheepAsync(CheepDTO cheepDto)
        {
            var author = await _dbContext.Authors.FindAsync(cheepDto.AuthorId);
            if (author == null)
            {
                throw new KeyNotFoundException("Author not found");
            }


            var cheep = new Cheep
            {
                Text = cheepDto.Message,
                TimeStamp = DateTime.Parse(cheepDto.TimeStamp),
                AuthorId = cheepDto.AuthorId,
                Author = author
            };
            
                author.Cheeps.Add(cheep);
            
            var queryResult = await _dbContext.Cheeps.AddAsync(cheep);

            await _dbContext.SaveChangesAsync();
            return queryResult.Entity.CheepId;
        }

        public async Task<CheepDTO?> DeleteCheepAsync(int id)
        {
            var cheep = await _dbContext.Cheeps.Include(c => c.Author).FirstOrDefaultAsync(c => c.CheepId == id);
            if (cheep != null)
            {
                cheep.Author.Cheeps.Remove(cheep);
                _dbContext.Cheeps.Remove(cheep);
                await _dbContext.SaveChangesAsync();
                return new CheepDTO
                {
                    Id = cheep.CheepId,
                    Name = cheep.Author.Name,
                    Message = cheep.Text,
                    TimeStamp = cheep.TimeStamp.ToString(),
                    AuthorId = cheep.AuthorId,
                    AuthorEmail = cheep.Author.Email
                };
            }
            return null;
        }

        public async Task<PagedResult<CheepDTO>> GetAllCheepsAsync(int page, int pageSize)
        {
            var query = _dbContext.Cheeps.Include(c => c.Author).Select(c => new CheepDTO
            {
                Id = c.CheepId,
                Name = c.Author.Name,
                Message = c.Text,
                TimeStamp = c.TimeStamp.ToString(),
                AuthorId = c.AuthorId,
                AuthorEmail = c.Author.Email
            });

            var totalCheeps = await query.CountAsync();
            var cheeps = await query.OrderByDescending(c => c.TimeStamp)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedResult<CheepDTO>
            {
                Items = cheeps,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCheeps / (double)pageSize)
            };
        }

        public async Task<PagedResult<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName, int page, int pageSize)
        {
            var query = _dbContext.Cheeps
                .Include(c => c.Author)
                .Where(c => c.Author.Name == authorName)
                .Select(c => new CheepDTO
                {
                    Id = c.CheepId,
                    Name = c.Author.Name,
                    Message = c.Text,
                    TimeStamp = c.TimeStamp.ToString(),
                    AuthorId = c.AuthorId,
                    AuthorEmail = c.Author.Email
                });

            var totalCheeps = await query.CountAsync();
            var cheeps = await query.OrderByDescending(c => c.TimeStamp)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedResult<CheepDTO>
            {
                Items = cheeps,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCheeps / (double)pageSize)
            };
        }

        public async Task<CheepDTO> GetCheepByIdAsync(int id)
        {
            return await _dbContext.Cheeps.Where(c => c.CheepId == id).Select(c => new CheepDTO
            {
                Id = c.AuthorId,
                Name = c.Author.Name,
                Message = c.Text,
                TimeStamp = c.TimeStamp.ToString(),
                AuthorId = c.AuthorId,
                AuthorEmail = c.Author.Email
            }).FirstOrDefaultAsync();
        }


        public async Task UpdateCheepAsync(CheepDTO cheepDto)
        {
            var cheep = await _dbContext.Cheeps.FindAsync(cheepDto.Id);
            if (cheep != null)
            {
                cheep.Text = cheepDto.Message;
                cheep.TimeStamp = DateTime.Parse(cheepDto.TimeStamp);

                if (cheep.AuthorId != cheepDto.AuthorId)
                {
                    cheep.Author.Cheeps.Remove(cheep);
                    var newAuthor = await _dbContext.Authors.FindAsync(cheepDto.AuthorId);
                    if (newAuthor != null)
                    {
                        newAuthor.Cheeps.Add(cheep);
                        cheep.Author = newAuthor;
                    }
                }

                _dbContext.Cheeps.Update(cheep);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
