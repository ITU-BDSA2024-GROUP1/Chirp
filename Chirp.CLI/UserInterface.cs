using SimpleDB;

namespace Chirp.CLI
{
    internal class UserInterface
    {
        static string cheepsCsvPath = String.Empty;

        private static IDatabaseRepository<Cheep>? cheepBase; 

        public static void SetCheepsCsvPath(string path) 
        { 
            cheepsCsvPath = path; 
            cheepBase = new CSVDatabase<Cheep>(cheepsCsvPath);
        }

        public static void ReadCheeps(int? value)
        {
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
            Cheep cheep = new(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now));
            cheepBase.Store(cheep);
        }

        private static long ParseDateTimeToUnixTime(DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}