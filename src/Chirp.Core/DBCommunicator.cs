using SimpleDB;

namespace Chirp.Core;

public class DBCommunicator
{
    private readonly IDatabaseRepository<Cheep> _repo;

    public DBCommunicator(IDatabaseRepository<Cheep> repository) => _repo = repository;

    public IEnumerable<Cheep> ReadCheeps(int? limit = null) => _repo.Read(limit);

    public void WriteCheep(string message)
    {
        Cheep cheep = new(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now));
        _repo.Store(cheep);

        return;
        
        static long ParseDateTimeToUnixTime(DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}