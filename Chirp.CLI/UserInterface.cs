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

using SimpleDB;

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

        public static IDatabaseRepository<Cheep> cheepBase; 

        public static void setCheepsCsvPath(string path) 
        { 
            cheepsCsvPath = path; 
            cheepBase = new CSVDatabase<Cheep>(cheepsCsvPath);
        }

        public static async Task ReadCheeps()
        {
            PrintCheeps(new List<Cheep>(cheepBase.Read()));
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
            cheepBase.Store(cheep);
        }


        static long ParseDateTimeToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
    }
}
