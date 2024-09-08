using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.CommandLine;
using SimpleDB;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chirp.CLI
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Chirp where you can send cheeps and read others");

            var readCommand = new Command("read", "Read informantion stored in database");
            readCommand.SetHandler(async () =>
            {
                await Read();
            });

            var storeCommand = new Command("cheep", "Add a cheep to the databas");
            var cheepArgument = new Argument<string>("message", "The cheep you want to send");
            storeCommand.AddArgument(cheepArgument);
            storeCommand.SetHandler(async (string message) =>
             {
                 await Cheep(message);   
             }, cheepArgument);

            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(storeCommand);

            return await rootCommand.InvokeAsync(args);
        }

        /**
        * Reads the 
        */
        static async Task Read()
        {
            string[] cheepsIn = await File.ReadAllLinesAsync("data/chirp_cli_db.csv");
            string[] cheeps = new string[cheepsIn.Length - 1];
            for (int i = 0; i < cheepsIn.Length-1; i++)
            {
                if (cheepsIn[i+1].Length == 0) continue;
                cheeps[i] = ParseCheep(cheepsIn[i+1]);
                Console.WriteLine(cheeps[i]);
            }
        }
        // Line 51
        static string ParseCheep(string cheep)
        {
            // Help from https://www.codeproject.com/Answers/555757/C-23plusString-FormatplusAlignment
            string[] cheepContent = Regex.Split(cheep, @",\""|\"",");
            DateTime date = ParseUnixTimeToDateTime(long.Parse(cheepContent[2]));
            return $"{cheepContent[0], -10} @ {date.ToString("MM/dd/yy HH:mm:ss"), 17}: {cheepContent[1]}";
        }

        // UNIX timestamp help https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        static DateTime ParseUnixTimeToDateTime(long date)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(date).ToLocalTime();
            return dateTime;
        } // Line 72

        static long ParseDateTimeToUnixTime(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        
        static async Task Cheep(string content)
        {
            string csvLine = $"\n{Environment.UserName},\"{content}\",{ParseDateTimeToUnixTime(DateTime.UtcNow)}";
            await File.AppendAllTextAsync("data/chirp_cli_db.csv", csvLine);
        }
    }
}