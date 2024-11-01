namespace Chirp.Core.DataTransferObject;

public class AuthorDTO : IEquatable<AuthorDTO>
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
        
    public bool Equals(AuthorDTO? that)
    {
        return that != null && this.Id == that.Id && this.Name == that.Name && this.Email == that.Email;
    }
}