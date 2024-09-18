using Chirp.Core;

using SimpleDB;


namespace SimpleDBTest;

[Collection("Non-Parallel Collection")]
public class IntegrationTestDB
{
    const string Path = "data/test.csv";
    
    private record struct TestRecord(String Author, String Message, long Timestamp);

    private static IDatabaseRepository<TestRecord> TestBase => CSVDatabase<TestRecord>.Instance;
    
    [Fact]
    public void CSVDatabase_StoreRead()
    {
        //Arrange
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        const int limit = 1;
        
        TestRecord message = new ("test", "integration", 1);
        string expected = $"TestRecord {{ Author = test, Message = integration, Timestamp = 1 }}{Environment.NewLine}";
        CSVDatabase<TestRecord>.InTestingDatabase = true;
        TestBase.Store(message);
            
        //ACT
        IEnumerable<TestRecord> result = TestBase.Read(limit);
            

        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
                
            foreach (TestRecord r in result) Console.WriteLine(r);
            
            Assert.Equal(expected, sw.ToString());

        }
            
        
        String[] strings = File.ReadAllLines(Path);
        File.WriteAllLines(Path, strings.Take(strings.Length - 1).ToArray());

        strings = File.ReadAllLines(Path);
        Assert.DoesNotContain(message.ToString(), strings[^1]);
    }
}