using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

using static Chirp.CLI.UserInterface;

namespace Chirp.CLI
{
    internal class UserInterface
    {
        public record Cheep(string Author, string Message, long Timestamp) 
        { 
            override public string ToString()
            {
                // Help from https://www.codeproject.com/Answers/555757/C-23plusString-FormatplusAlignment
                DateTime date = ParseUnixTimeToDateTime(Timestamp);
                return $"{Author,-15} @ {date.ToString("MM/dd/yy HH:mm:ss"),17}: {Message}";
            }
        }

        static string cheepsCsvPath = String.Empty;

        public static void setCheepsCsvPath(string path) { cheepsCsvPath = path; }

        public static async Task ReadCheeps()
        {
            List<Cheep> cheeps = GetCheepsFromCsv();
            PrintCheeps(cheeps);
        }

        static List<Cheep> GetCheepsFromCsv()
        {
            List<Cheep> cheeps;
            using (StreamReader reader = new StreamReader(cheepsCsvPath))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                cheeps = new List<Cheep>(csv.GetRecords<Cheep>());
            }
            return cheeps;
        }


        static void PrintCheeps(List<Cheep> cheeps)
        {
            foreach (Cheep cheep in cheeps)
            {
                Console.WriteLine(cheep.ToString());
            }
        }

        // UNIX timestamp help https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        static DateTime ParseUnixTimeToDateTime(long date)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(date).ToLocalTime();
            return dateTime;
        }

        public static async Task WriteCheep(string message)
        {
            Cheep cheep = new Cheep(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now));
            List<Cheep> cheeps = new List<Cheep>();
            cheeps.Add(cheep);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(cheepsCsvPath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer,config))
            {
                csv.WriteRecords(cheeps);
            }
        }


        static long ParseDateTimeToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
    }
}
