using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.Models;
using Chirp.Core.Repositories;

namespace Chirp.Infrastructure;

public record AuthorViewModel(int Id, string Name, string Email);
public interface IAuthorService
{
    public Task<AuthorViewModel> GetAuthorFromName(string author);
    public Task<AuthorViewModel> GetAuthorFromEmail(string email);
}
public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorViewModel> GetAuthorFromName(string author)
    {
        var authorResult = await _authorRepository.GetAuthorByNameAsync(author);
        return new AuthorViewModel(authorResult.Id, authorResult.Name, authorResult.Email);
    }

    public async Task<AuthorViewModel> GetAuthorFromEmail(string email)
    {
        var authorResult = await _authorRepository.GetAuthorByEmailAsync(email);
        return new AuthorViewModel(authorResult.Id, authorResult.Name, authorResult.Email);
    }
}
