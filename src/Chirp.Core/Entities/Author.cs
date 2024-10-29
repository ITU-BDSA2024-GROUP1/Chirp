using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;
public class Author : IdentityUser
{
    public int Id { get; set; } // Primary Key
    public required string  Name { get; set; }
    public List<Cheep> Cheeps { get; set; } = new List<Cheep>();
}
