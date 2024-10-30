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

        public async Task<string> AddAuthorAsync(AuthorDTO authorDto)
        {
            var author = new Author
            {
                UserName = authorDto.Name,
                Email = authorDto.Email
            };
            
            var queryResult = await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return queryResult.Entity.Id;
        }

        public async Task<AuthorDTO> DeleteAuthorAsync(string id)
        {
            var author = await _dbContext.Authors.FindAsync(id);
            if (author != null)
            {
                _dbContext.Authors.Remove(author);
                await _dbContext.SaveChangesAsync();
                return new AuthorDTO
                {
                    Id = author.Id,
                    Name = author.UserName,
                    Email = author.Email
                };
            }
            return null;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync()
        {
            return await _dbContext.Authors.Select(c => new AuthorDTO
            {
                Id = c.Id,
                Name = c.UserName,
                Email = c.Email
            }).ToListAsync();
        }

        public async Task<AuthorDTO> GetAuthorByIdAsync(string id)
        {
            return await _dbContext.Authors.Where(a => a.Id == id).Select(c => new AuthorDTO
            {
                Id = c.Id,
                Name = c.UserName,
                Email = c.Email
            }).FirstOrDefaultAsync();
        }

        public async Task UpdateAuthorAsync(AuthorDTO authorDto)
        {
            var author = await _dbContext.Authors.FindAsync(authorDto.Id);
            if (author != null)
            {
                author.UserName = authorDto.Name;
                author.Email = authorDto.Email;
                _dbContext.Authors.Update(author);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
