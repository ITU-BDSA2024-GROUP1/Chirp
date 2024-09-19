using Chirp.Core;

using SimpleDB;

namespace SimpleDBTest;

[Collection("SimpleDB Collection")]
public class IntegrationTestDB : SimpleDBTester
{
    public IntegrationTestDB(SimpleDBFixture fixture) : base(fixture) { }

    [Fact]
    public void CSVDatabase_StoreRead()
    {
        //Arrange
        Cheep message = new("integration", "test", 1);
        string expected = message.ToString();
        
        _fixture.TestBase.Store(message);
            
        //ACT
        Cheep result = _fixture.TestBase.Read(1).First();
        Assert.Equal(expected, result.ToString());
    }
}