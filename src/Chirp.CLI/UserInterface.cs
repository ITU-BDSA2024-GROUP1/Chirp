using Chirp.Core;

namespace Chirp.CLI;

internal static class UserInterface
{
    private static readonly DBCommunicator Communicator = new DBCommunicator(WebDB<Cheep>.Instance);
    
    public static void ReadCheeps(int? limit = null)
    {
        var cheeps = Communicator.ReadCheeps(limit);
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(cheep);
        }
    }

    public static void WriteCheep(string message) => Communicator.WriteCheep(message);
}