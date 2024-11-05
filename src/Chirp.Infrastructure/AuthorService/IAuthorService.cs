namespace Chirp.Infrastructure.AuthorService;

public interface IAuthorService
{
    public Task<AuthorViewModel> GetAuthorByName(string author);
    public Task<AuthorViewModel> GetAuthorByEmail(string email);
    public Task<AuthorViewModel> GetAuthorById(string id);
    public Task<string> CreateAuthor(AuthorViewModel author);
}