using Chirp.Razor;

public record CheepViewModel(string Author, string Message, long Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author,int page);
}

public class CheepService : ICheepService
{
    private readonly DBFacade _dbFacade;

    public CheepService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }
    
    public List<CheepViewModel> GetCheeps(int page)
    {
        
        string query = @"
            SELECT u.username, m.text, m.pub_date 
            FROM message m 
            JOIN user u ON m.author_id = u.user_id"
                       +$" ORDER BY m.pub_date DESC LIMIT {32} OFFSET {32 * page}";
        return _dbFacade.ExecuteQuery(query);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        string query = @"
            SELECT u.username, m.text, m.pub_date 
            FROM message m 
            JOIN user u ON m.author_id = u.user_id 
            WHERE u.username = @Author"
                       +$" ORDER BY m.pub_date DESC LIMIT {32} OFFSET {32 * page}";
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
