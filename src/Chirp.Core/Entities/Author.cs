namespace Chirp.Core.Entities;

public class Author
{
    public int AuthorId { get; set; } // Primary Key
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps { get; set; } = [];
}
