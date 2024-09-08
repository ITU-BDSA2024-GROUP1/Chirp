using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chirp.CLI
{
    internal class UserInterface
    {
        static string cheepsCsvPath = String.Empty;

        public static void setCheepsCsvPath(string path) { cheepsCsvPath = path; }

        public static async Task ReadCheeps()
        {
            string[] cheeps = GetCheepsFromCsv();
            ParseCheepsToOutput(cheeps);
            PrintCheeps(cheeps);
        }

        static string[] GetCheepsFromCsv()
        {
            string[] cheepsIn = File.ReadAllLines(cheepsCsvPath);
            string[] cheeps = new string[cheepsIn.Length];
            int actualCheeps = 0;
            for (int i = 1; i < cheepsIn.Length; i++)
            {
                if (cheepsIn[i].Length == 0) continue;
                cheeps[actualCheeps++] = cheepsIn[i];
            }
            string[] cheepsOut = new string[actualCheeps];
            Array.Copy(cheeps, cheepsOut, actualCheeps);

            return cheepsOut;
        }

        static void ParseCheepsToOutput(string[] cheeps)
        {
            for (int i = 0; i < cheeps.Length; i++)
            {
                string cheep = ParseCheep(cheeps[i]);
                cheeps[i] = cheep;
            }
        }

        static void PrintCheeps(string[] cheeps)
        {
            foreach (string cheep in cheeps)
            {
                Console.WriteLine(cheep);
            }
        }

        static string ParseCheep(string cheep)
        {
            // Help from https://www.codeproject.com/Answers/555757/C-23plusString-FormatplusAlignment
            string[] cheepContent = Regex.Split(cheep, @",\""|\"",");
            DateTime date = ParseUnixTimeToDateTime(long.Parse(cheepContent[2]));
            return $"{cheepContent[0],-10} @ {date.ToString("MM/dd/yy HH:mm:ss"),17}: {cheepContent[1]}";
        }

        // UNIX timestamp help https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        static DateTime ParseUnixTimeToDateTime(long date)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(date).ToLocalTime();
            return dateTime;
        } 
    }
}
