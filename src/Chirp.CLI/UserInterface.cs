using Chirp.Core;

using SimpleDB;

namespace Chirp.CLI;

public class UserInterface
{
    private readonly IDatabaseRepository<Cheep> _repo;
    
    public UserInterface(IDatabaseRepository<Cheep> repository) => _repo = repository;

    public void ReadCheeps(int? limit = null)
    {
        var cheeps = _repo.Read(limit);
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(cheep);
        }
    }

    public void WriteCheep(string message) => _repo.Store(new Cheep(message));
}