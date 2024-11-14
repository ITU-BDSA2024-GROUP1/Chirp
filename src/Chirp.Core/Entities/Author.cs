using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

public class Author : IdentityUser
{
    public List<Cheep> Cheeps { get; set; }
    public string LoginProvider { get; set; } = null;
    public string ProviderKey { get; set; } = null;
}