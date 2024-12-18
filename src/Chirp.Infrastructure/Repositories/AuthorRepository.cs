using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class AuthorRepository(ChirpDBContext dbContext) : IAuthorRepository
{
    public async Task<string> AddAuthorAsync(AuthorDTO authorDto)
    {
        var author = new Author
        {
            Id = authorDto.Id,
            UserName = authorDto.Name,
            Email = authorDto.Email
        };
            
        var queryResult = await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        return queryResult.Entity.Id;
    }

    public async Task<AuthorDTO> DeleteAuthorAsync(string id)
    {
        var author = await dbContext.Authors.FindAsync(id);
        if (author == null) return null;

        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();
        return new()
        {
            Id = author.Id,
            Name = author.UserName,
            Email = author.Email
        };
    }

    public async Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync()
    {
        return await dbContext.Authors
            .Select(a => new AuthorDTO
        {
            Id = a.Id,
            Name = a.UserName,
            Email = a.Email
        }).ToListAsync();
    }

    public async Task<AuthorDTO> GetAuthorByIdAsync(string id)
    {
        return await dbContext.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorDTO
        {
            Id = a.Id,
            Name = a.UserName,
            Email = a.Email
        }).FirstOrDefaultAsync();
    }
        
    public async Task<AuthorDTO> GetAuthorByNameAsync(string name)
    {
        return await dbContext.Authors
            .Where(a => a.UserName == name)
            .Select(a => new AuthorDTO
        {
            Id = a.Id, Name = a.UserName, Email = a.Email
        }).FirstOrDefaultAsync();
    }

    public async Task<AuthorDTO> GetAuthorByEmailAsync(string email)
    {
        return await dbContext.Authors
            .Where(a=> a.Email == email)
            .Select(a => new AuthorDTO
        {
            Id = a.Id,
            Name = a.UserName,
            Email = a.Email
        }).FirstOrDefaultAsync();
    }

    public async Task UpdateAuthorAsync(AuthorDTO authorDto)
    {
        var author = await dbContext.Authors.FindAsync(authorDto.Id);
        if (author == null) return;
        
        author.UserName = authorDto.Name;
        author.Email = authorDto.Email;
        dbContext.Authors.Update(author);
        await dbContext.SaveChangesAsync();
    }
}