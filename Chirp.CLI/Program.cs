using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Chirp.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "read":
                    Read();
                    break;
                case "cheep":
                    Cheep(args[1]);
                    break;
                default:
                    break;
            }
        }

        static void Read()
        {
            string[] cheepsIn = File.ReadAllLines("data/chirp_cli_db.csv");
            string[] cheeps = new string[cheepsIn.Length - 1];
            for (int i = 0; i < cheepsIn.Length-1; i++)
            {
                if (cheepsIn[i+1].Length == 0) continue;
                cheeps[i] = ParseCheep(cheepsIn[i+1]);
                Console.WriteLine(cheeps[i]);
            }
        }

        static string ParseCheep(string cheep)
        {
            string[] cheepContent = Regex.Split(cheep, @",\""|\"",");
            StringBuilder SB = new StringBuilder();
            SB.Append(cheepContent[0]);
            SB.Append(" @ ");
            
            DateTime date = ParseUnixTimeToDateTime(long.Parse(cheepContent[2]));
            SB.Append(date.ToString("MM/dd/yy HH:mm:ss"));
            
            SB.Append(": " + cheepContent[1]);
            return SB.ToString();
        }

        // UNIX timestamp help https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        static DateTime ParseUnixTimeToDateTime(long date)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(date).ToLocalTime();
            return dateTime;
        }

        static long ParseDateTimeToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        
        static void Cheep(string content)
        {
            string csvLine = $"\n{Environment.UserName},\"{content}\",{ParseDateTimeToUnixTime(DateTime.UtcNow)}";
            File.AppendAllText("data/chirp_cli_db.csv", csvLine);
        }
    }
}