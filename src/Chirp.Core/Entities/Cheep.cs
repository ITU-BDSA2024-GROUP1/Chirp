namespace Chirp.Core.Entities;

public class Cheep
{
    public int CheepId { get; set; } // Primary Key
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; } // Foreign Key
    public required Author Author { get; set; }
}