using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

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

            if (author != null)
            {
                author.Cheeps.Add(cheep);
            }

            var queryResult = await _dbContext.Cheeps.AddAsync(cheep);

            await _dbContext.SaveChangesAsync();
            return queryResult.Entity.CheepId;
        }

        public async Task<CheepDTO> DeleteCheepAsync(int id)
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
                    AuthorId = cheep.AuthorId
                };
            }
            return null;
        }

        public async Task<IEnumerable<CheepDTO>> GetAllCheepsAsync()
        {
            return await _dbContext.Cheeps.Include(c => c.Author).Select(c => new CheepDTO
            {
                Id = c.CheepId,
                Name = c.Author.Name,
                Message = c.Text,
                TimeStamp = c.TimeStamp.ToString(),
                AuthorId = c.AuthorId
            }).ToListAsync();
        }

        public async Task<IEnumerable<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName)
        {
            return await _dbContext.Cheeps
                .Include(c => c.Author)
                .Where(c => c.Author.Name == authorName)
                .Select(c => new CheepDTO
                {
                    Id = c.CheepId,
                    Name = c.Author.Name,
                    Message = c.Text,
                    TimeStamp = c.TimeStamp.ToString(),
                    AuthorId = c.AuthorId
                }).ToListAsync();
        }

        public async Task UpdateCheepAsync(CheepDTO cheepDto)
        {
            var cheep = await _dbContext.Cheeps.Include(c => c.Author).FirstOrDefaultAsync(c => c.CheepId == cheepDto.Id);
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
