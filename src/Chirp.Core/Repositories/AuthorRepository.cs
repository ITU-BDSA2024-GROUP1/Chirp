using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _dbContext;

        public AuthorRepository(ChirpDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAuthorAsync(AuthorDTO authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name,
                Email = authorDto.Email
            };
            
            var queryResult = await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return queryResult.Entity.AuthorId;
        }

        public async Task<AuthorDTO> DeleteAuthorAsync(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);
            if (author != null)
            {
                _dbContext.Authors.Remove(author);
                await _dbContext.SaveChangesAsync();
                return new AuthorDTO
                {
                    Id = author.AuthorId,
                    Name = author.Name,
                    Email = author.Email
                };
            }
            return null;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync()
        {
            return await _dbContext.Authors.Include(c => c.AuthorId).Select(c => new AuthorDTO
            {
                Id = c.AuthorId,
                Name = c.Name,
                Email = c.Email
            }).ToListAsync();
        }

        public async Task<AuthorDTO> GetAuthorByIdAsync(int id)
        {
            return await _dbContext.Authors.Where(a => a.AuthorId == id).Select(c => new AuthorDTO
            {
                Id = c.AuthorId,
                Name = c.Name,
                Email = c.Email
            }).FirstOrDefaultAsync();
        }

        public async Task UpdateAuthorAsync(AuthorDTO authorDto)
        {
            var author = await _dbContext.Authors.FindAsync(authorDto.Id);
            if (author != null)
            {
                author.Name = authorDto.Name;
                author.Email = authorDto.Email;
                _dbContext.Authors.Update(author);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
