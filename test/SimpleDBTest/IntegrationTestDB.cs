using System.IO;

using Chirp.CLI;

using SimpleDB;


namespace SimpleDBTest;

public class IntegrationTestDB
{
    private record struct test_record(String Author, String Message , long Timestamp);

    private static IDatabaseRepository<test_record> TestBase  => CSVDatabase<test_record>.Instance;
   

    [Fact]
    public void CSVDatabase_StoreRead()
    {
        //Arrange
        //Program.SetWorkingDirectoryToProjectRoot();
        while (Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
        {
            Directory.SetCurrentDirectory(Path.GetFullPath(".."));
        }
        int limit = 1;
        string path = "data/test.csv";
        test_record message = new ("test", "integration", 1) ;
        string expected = "test_record { Author = test, Message = integration, Timestamp = 1 }\r\n";
        CSVDatabase<test_record>.InTestingDatabase = true;
        TestBase.Store(message);
            
        //ACT
        IEnumerable<test_record> result = TestBase.Read(limit);
            

        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
                
            foreach (test_record r in result){Console.WriteLine(r);}
                
                
            Assert.Equal(expected, sw.ToString());

        }
            
        
        String[] strings = File.ReadAllLines(path);
        File.WriteAllLines(path, strings.Take(strings.Length - 1).ToArray());

        strings = File.ReadAllLines(path);
        Assert.DoesNotContain(message.ToString(), strings[strings.Length - 1]);
    }
}