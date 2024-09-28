using Chirp.Razor;

public record CheepViewModel(string Author, string Message, long Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService(DBFacade dbFacade) : ICheepService
{
    private readonly DBFacade _dbFacade = dbFacade;

    public List<CheepViewModel> GetCheeps()
    {
        string query = @"
            SELECT u.username, m.text, m.pub_date 
            FROM message m 
            JOIN user u ON m.author_id = u.user_id";
        return _dbFacade.ExecuteQuery(query);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        string query = @"
            SELECT u.username, m.text, m.pub_date 
            FROM message m 
            JOIN user u ON m.author_id = u.user_id 
            WHERE u.username = @Author";
        return _dbFacade.ExecuteQuery(query, author);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
