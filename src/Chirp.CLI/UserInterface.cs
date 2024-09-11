using SimpleDB;

namespace Chirp.CLI
{
    internal class UserInterface
    {
        private static IDatabaseRepository<Cheep>? cheepBase;

        public static void SetCheepsCsvPath(string path)
        { 
            cheepBase = new CSVDatabase<Cheep>(path);
        }

        public static void ReadCheeps(int? value)
        {
            if (cheepBase == null) throw new InvalidOperationException("ReadCheeps() has been called, but the database hasn't been initialized and therefore currently doesn't exist.");

            var cheeps = cheepBase.Read(value);
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
            if (cheepBase == null) throw new InvalidOperationException("WriteCheep() has been called, but the database hasn't been initialized and therefore currently doesn't exist.");

            Cheep cheep = new(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now));
            cheepBase.Store(cheep);
        }

        private static long ParseDateTimeToUnixTime(DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}