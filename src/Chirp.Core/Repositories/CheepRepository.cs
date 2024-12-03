using System.ComponentModel.DataAnnotations;

using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;
using Chirp.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Repositories;

public class CheepRepository(ChirpDBContext dbContext) : ICheepRepository
{
    public async Task<int> AddCheepAsync(CheepDTO cheepDto)
    {
        var author = await dbContext.Authors.Include(a => a.Cheeps).FirstOrDefaultAsync(a => a.Id == cheepDto.AuthorId); ;
        if (author == null) throw new KeyNotFoundException("Author not found");
        
        var cheep = new Cheep
        {
            Text = cheepDto.Message,
            TimeStamp = DateTime.Parse(cheepDto.TimeStamp),
            AuthorId = cheepDto.AuthorId,
            Author = author
        };

        // Validate the Cheep entity
        var validationContext = new ValidationContext(cheep); // Provides contextual information about the Cheep object being validated
        var validationResults = new List<ValidationResult>(); // Stores any validation errors that occur during the validation process

        if (!Validator.TryValidateObject(cheep, validationContext, validationResults, true)) // Validates the Cheep object based on data annotations. Recursively validates all properties
        {
            var messages = string.Join("; ", validationResults.Select(r => r.ErrorMessage)); // Joins all error messages into a single string, separated by semicolons
            throw new ValidationException(messages); // Throws a ValidationException with the concatenated error messages
        }

        author.Cheeps.Add(cheep);

        var queryResult = await dbContext.Cheeps.AddAsync(cheep);

        await dbContext.SaveChangesAsync();
        return queryResult.Entity.CheepId;
    }

    public async Task<CheepDTO> DeleteCheepAsync(int id)
    {
        var cheep = await dbContext.Cheeps.Include(c => c.Author).FirstOrDefaultAsync(c => c.CheepId == id);
        if (cheep == null) return null;

        var author = cheep.Author; 
        author.Cheeps.Remove(cheep);

        dbContext.Cheeps.Remove(cheep);
            
        CheepDTO deletedCheep = new()
        {
            Id = cheep.CheepId,
            Name = cheep.Author.UserName,
            Message = cheep.Text,
            TimeStamp = cheep.TimeStamp.ToString(),
            AuthorId = cheep.AuthorId,
            AuthorEmail = cheep.Author.Email
        };

        await dbContext.SaveChangesAsync();
            
        return deletedCheep;
    }

    public async Task<PagedResult<CheepDTO>> GetAllCheepsAsync(int page, int pageSize)
    {
        var query = dbContext.Cheeps.Include(c => c.Author).Select(c => new CheepDTO
        {
            Id = c.CheepId,
            Name = c.Author.UserName,
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

        return new()
        {
            Items = cheeps,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCheeps / (double)pageSize)
        };
    }

    public async Task<PagedResult<CheepDTO>> GetCheepsByAuthorNameAsync(string authorName, int page, int pageSize)
    {
        var query = dbContext.Cheeps
            .Include(c => c.Author)
            .Where(c => c.Author.UserName == authorName)
            .Select(c => new CheepDTO
            {
                Id = c.CheepId,
                Name = c.Author.UserName,
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

        return new()
        {
            Items = cheeps,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCheeps / (double)pageSize)
        };
    }

    public async Task<PagedResult<CheepDTO>> GetCheepsByAuthorNameAsync(List<string> authorNames, int page, int pageSize)
    {
        var query = dbContext.Cheeps
            .Include(c => c.Author)
            .Where(c => authorNames.Contains(c.Author.UserName))
            .Select(c => new CheepDTO
            {
                Id = c.CheepId,
                Name = c.Author.UserName,
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

        return new()
        {
            Items = cheeps,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCheeps / (double)pageSize)
        };
    }

    public async Task<CheepDTO> GetCheepByIdAsync(int id)
    {
        return await dbContext.Cheeps.Where(c => c.CheepId == id).Select(c => new CheepDTO
        {
            Id = c.CheepId,
            Name = c.Author.UserName,
            Message = c.Text,
            TimeStamp = c.TimeStamp.ToString(),
            AuthorId = c.AuthorId,
            AuthorEmail = c.Author.Email
        }).FirstOrDefaultAsync();
    }


    public async Task UpdateCheepAsync(CheepDTO cheepDto)
    {
        var cheep = await dbContext.Cheeps.FindAsync(cheepDto.Id);
        if (cheep == null) return;

        var oldAuthor = cheep.Author;
        cheep.Text = cheepDto.Message;
        cheep.TimeStamp = DateTime.Parse(cheepDto.TimeStamp);

        if (cheep.AuthorId != cheepDto.AuthorId)
        {
            var newAuthor = await dbContext.Authors.FindAsync(cheepDto.AuthorId);
            if (newAuthor != null)
            {
                oldAuthor.Cheeps.Remove(cheep);
                newAuthor.Cheeps.Add(cheep);
                cheep.Author = newAuthor;
            }
        }

        dbContext.Cheeps.Update(cheep);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> GetCheepCountByAuthor(string authorName)
    {
        return await dbContext.Cheeps.Where(c => c.Author.UserName == authorName).CountAsync();
    }
}