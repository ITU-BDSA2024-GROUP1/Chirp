namespace Chirp.CLI;

internal static class UserInterface
{
    public static void ReadCheeps(int? limit = null)
    {
        var cheeps = DBCommunicator.ReadCheeps(limit);
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(cheep);
        }
    }

    public static void WriteCheep(string message) => DBCommunicator.WriteCheep(message);
}