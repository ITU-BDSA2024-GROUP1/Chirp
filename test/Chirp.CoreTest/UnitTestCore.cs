using Chirp.Core;
/*
namespace Chirp.CoreTest;

public class UnitTestCore
{
    const string Author = "Author", Message = "Message";
    const long Timestamp = 1627846261;
    
    [Fact]
    public void Cheep_Instantiation()
    {
        // Act
        var cheep = new Cheep(Author, Message, Timestamp);

        // Assert
        Assert.Equal(Author, cheep.Author);
        Assert.Equal(Message, cheep.Message);
        Assert.Equal(Timestamp, cheep.Timestamp);
    }

    [Fact]
    public void Cheep_ToString()
    {
        // Arrange
        var cheep = new Cheep(Author, Message, Timestamp);
        const string expected = "Author          @ 08/01/21 19:31:01: Message";

        // Act
        string result = cheep.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Author", "Message", 0, "Author          @ 01/01/70 00:00:00: Message")] // Unix epoch start
    [InlineData("Author", "Message", 253402300799, "Author          @ 12/31/99 23:59:59: Message")] // End of 9999 year
    [InlineData("Author", "Message", 10000000000, "Author          @ 11/20/86 17:46:40: Message")] // Far future date
    [InlineData("Author", "Message", -315619200, "Author          @ 01/01/60 00:00:00: Message")] // Date before Unix epoch
    public void Cheep_ToString_ExtremeTimestamps(string author, string message, long timestamp, string expected)
    {
        // Arrange
        var cheep = new Cheep(author, message, timestamp);

        // Act
        string result = cheep.ToString();

        // Assert
        Assert.Equal(expected, result);
    }
}
*/