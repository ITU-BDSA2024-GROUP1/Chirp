using Chirp.Core;

using SimpleDB;

namespace Chirp.CLI;

public class UserInterface(IDatabaseRepository<Cheep> repository)
{
    public void ReadCheeps(int? limit = null)
    {
        var cheeps = repository.Read(limit);
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(cheep);
        }
    }

    public void WriteCheep(string message)
    {
        Cheep cheep = new(message);
        repository.Store(cheep);
    }
}