using Chirp.Core;

namespace SimpleDBTest;

[Collection("SimpleDB Collection")]
public class UnitTestDB : SimpleDBTester
{
    public UnitTestDB(SimpleDBFixture fixture) : base(fixture) { }

    [Fact]
    public void SimpleDB_Instantiation()
    {
        //Act
        Assert.NotNull(_fixture.TestBase);
    }

    [Fact]
    public void SimpleDB_Read()
    {
        //Arrange
        var results = _fixture.TestBase.Read();
            
        //Act
        Assert.NotNull(results);
    }

    [Fact]
    public void SimpleDB_Store()
    {
        //Arrange
        _fixture.TestBase.Store(new Cheep("AuthorDB", "DBMessage", 0));
            
        //Act
        int newDBLength = ReadFile().Length;
        Assert.Equal(_originalDBLength + 1, newDBLength);
    }
}