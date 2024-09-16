using SimpleDB;

namespace Chirp.CLI
{
    internal static class UserInterface
    {
        private static IDatabaseRepository<Cheep> CheepBase => CSVDatabase<Cheep>.Instance;
       
        public static void ReadCheeps(int? value)
        {
            var cheeps = CheepBase.Read(value);
            PrintCheeps(cheeps);
        }
        
        private static void PrintCheeps(IEnumerable<Cheep> cheeps)
        {
            foreach (Cheep cheep in cheeps)
            {
                Console.WriteLine(cheep);
            }
        }

        public static void WriteCheep(string message)
        {
            Cheep cheep = new(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now));
            CheepBase.Store(cheep);
        }

        private static long ParseDateTimeToUnixTime(DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}