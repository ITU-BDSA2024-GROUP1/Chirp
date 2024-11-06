namespace Chirp.Core.DataTransferObject;

public class AuthorDTO : IEquatable<AuthorDTO>
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
        
    public bool Equals(AuthorDTO that) => that != null && this.Id == that.Id;
}