namespace Chirp.Core.Models;

public class PagedResult<T>
{
    public required List<T> Items { get; set; }
    public required int CurrentPage { get; set; }
    public required int TotalPages { get; set; }
}