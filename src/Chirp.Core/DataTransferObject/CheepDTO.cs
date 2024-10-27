namespace Chirp.Core.DataTransferObject
{
    public class CheepDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Message { get; set; }
        public required string TimeStamp { get; set; }
        public required int AuthorId { get; set; }
        public required string AuthorEmail { get; set; }
    }
}
