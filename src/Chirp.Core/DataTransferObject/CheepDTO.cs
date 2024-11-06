namespace Chirp.Core.DataTransferObject;

public class CheepDTO : IEquatable<CheepDTO>
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Message { get; set; }
    public required string TimeStamp { get; set; }
    public required string AuthorId { get; set; }
    public required string AuthorEmail { get; set; }
        
    public bool Equals(CheepDTO that) => that != null && this.Id == that.Id;
}