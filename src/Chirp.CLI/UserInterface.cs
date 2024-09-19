using Chirp.Core;

using SimpleDB;

namespace Chirp.CLI;

public class UserInterface
{
    private readonly DBCommunicator _communicator;
    
    public UserInterface(IDatabaseRepository<Cheep> repository) => _communicator = new DBCommunicator(repository);

    public void ReadCheeps(int? limit = null)
    {
        var cheeps = _communicator.ReadCheeps(limit);
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(cheep);
        }
    }

    public void WriteCheep(string message) => _communicator.WriteCheep(message);
}