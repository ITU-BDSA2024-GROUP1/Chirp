﻿namespace Chirp.Core;

public record Cheep(string Author, string Message, long Timestamp)
{
    public Cheep() : this(string.Empty, string.Empty, 0) { }
    public Cheep(string message) : this(Environment.UserName, message, ParseDateTimeToUnixTime(DateTime.Now)) { }
    
    override public string ToString()
    {
        // Help from https://www.codeproject.com/Answers/555757/C-23plusString-FormatplusAlignment
        DateTime date = ParseUnixTimeToDateTime(Timestamp);
        return $@"{Author,-15} @ {date,17:MM\/dd\/yy HH\:mm\:ss}: {Message}";
    }
    
    // UNIX timestamp help https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
    private static DateTime ParseUnixTimeToDateTime(long date)
    {
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(date);
        
        return dateTime;
    }
    
    private static long ParseDateTimeToUnixTime(DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
}