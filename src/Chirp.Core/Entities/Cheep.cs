using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

public class Cheep
{
    public int CheepId { get; set; } // Primary Key
    [StringLength(250)]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public string AuthorId { get; set; } // Foreign Key
    public required IdentityUser Author { get; set; }
}