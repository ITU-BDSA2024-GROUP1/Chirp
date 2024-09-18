using System.IO;

using Chirp.CLI;

using SimpleDB;


namespace SimpleDBTest;

[Collection("Non-Parallel Collection")]
public class IntegrationTestDB
{
    private record struct TestRecord(String Author, String Message, long Timestamp);

    private static IDatabaseRepository<TestRecord> TestBase => CSVDatabase<TestRecord>.Instance;
    
    [Fact]
    public void CSVDatabase_StoreRead()
    {
        //Arrange
        Program.SetWorkingDirectoryToProjectRoot();
        int limit = 1;
        string path = "data/test.csv";
        TestRecord message = new ("test", "integration", 1);
        string expected = "TestRecord { Author = test, Message = integration, Timestamp = 1 }\r\n";
        CSVDatabase<TestRecord>.InTestingDatabase = true;
        TestBase.Store(message);
            
        //ACT
        IEnumerable<TestRecord> result = TestBase.Read(limit);
            

        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
                
            foreach (TestRecord r in result) { Console.WriteLine(r); }
            
            Assert.Equal(expected, sw.ToString());

        }
            
        
        String[] strings = File.ReadAllLines(path);
        File.WriteAllLines(path, strings.Take(strings.Length - 1).ToArray());

        strings = File.ReadAllLines(path);
        Assert.DoesNotContain(message.ToString(), strings[strings.Length - 1]);
    }
}