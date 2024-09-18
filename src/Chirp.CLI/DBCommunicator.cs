using SimpleDB;

namespace Chirp.CLI;

public static class DBCommunicator
{
    private static IDatabaseRepository<Cheep> CheepBase => CSVDatabase<Cheep>.Instance;

    public static IEnumerable<Cheep> ReadCheeps(int? limit = null) => CheepBase.Read(limit);

    public static void WriteCheep(string message)
    {
        Cheep cheep = new(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now));
        CheepBase.Store(cheep);

        return;
        
        static long ParseDateTimeToUnixTime(DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}