using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Repositories;

public class AuthorRepository(ChirpDBContext dbContext) : IAuthorRepository
{
    public async Task<int> AddAuthorAsync(AuthorDTO authorDto)
    {
        var author = new Author
        {
            Name = authorDto.Name,
            Email = authorDto.Email
        };
            
        var queryResult = await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        return queryResult.Entity.AuthorId;
    }

    public async Task<AuthorDTO?> DeleteAuthorAsync(int id)
    {
        var author = await dbContext.Authors.FindAsync(id);
        if (author == null) return null;

        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();
        return new()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email
        };
    }

    public async Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync()
    {
        return await dbContext.Authors.Select(c => new AuthorDTO
        {
            Id = c.AuthorId,
            Name = c.Name,
            Email = c.Email
        }).ToListAsync();
    }

    public async Task<AuthorDTO> GetAuthorByIdAsync(int id)
    {
        return await dbContext.Authors.Where(a => a.AuthorId == id).Select(c => new AuthorDTO
        {
            Id = c.AuthorId,
            Name = c.Name,
            Email = c.Email
        }).FirstOrDefaultAsync();
    }
        
    public async Task<AuthorDTO> GetAuthorByNameAsync(string name)
    {
        return await dbContext.Authors.Where(a => a.Name == name).Select(c => new AuthorDTO
        {
            Id = c.AuthorId, Name = c.Name, Email = c.Email
        }).FirstOrDefaultAsync();
    }

    public async Task<AuthorDTO> GetAuthorByEmailAsync(string email)
    {
        return await dbContext.Authors.Where(a=> a.Email == email).Select(c => new AuthorDTO
        {
            Id = c.AuthorId,
            Name = c.Name,
            Email = c.Email
        }).FirstOrDefaultAsync();
    }

    public async Task UpdateAuthorAsync(AuthorDTO authorDto)
    {
        var author = await dbContext.Authors.FindAsync(authorDto.Id);
        if (author == null) return;
        
        author.Name = authorDto.Name;
        author.Email = authorDto.Email;
        dbContext.Authors.Update(author);
        await dbContext.SaveChangesAsync();
    }
}