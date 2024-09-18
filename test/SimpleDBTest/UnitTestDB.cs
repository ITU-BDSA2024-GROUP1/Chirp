using System.IO;
using SimpleDB;
using Chirp.CLI;


namespace SimpleDBTest
{
    [Collection("Non-Parallel Collection")]
    public class UnitTestDB
    {
        private record DBtestRecord(String Author, String Message, long Timestamp);
        CSVDatabase<DBtestRecord> testbase;
        [Fact]
        public void SimpleDB_Instantiation()
        {
            //Arrange
            testbase = CSVDatabase<DBtestRecord>.Instance;
            //Assert
            Assert.NotNull(testbase);
        }

        [Fact]
        public void SimpleDB_read()
        {
            //Arrange
            testbase = CSVDatabase<DBtestRecord>.Instance;
            while (Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
            {
                Directory.SetCurrentDirectory(Path.GetFullPath(".."));
            }
            string path = "data/test.csv";
            CSVDatabase<DBtestRecord>.InTestingDatabase = true;
            IEnumerable<DBtestRecord> results =  testbase.Read(1);
            
            //Act
            Assert.NotNull(results);

        }

        [Fact]
        public void SimpleDB_store()
        {
            //Arrange
            testbase = CSVDatabase<DBtestRecord>.Instance;
            while (Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
            {
                Directory.SetCurrentDirectory(Path.GetFullPath(".."));
            }
            string path = "data/test.csv";
            String[] lines = File.ReadAllLines(path);
            int expected = lines.Length+1 ;
            CSVDatabase<DBtestRecord>.InTestingDatabase = true;
            testbase.Store(new DBtestRecord( "AuthorDB", "DBmessage", 0));
            
            //Act
            int result = File.ReadAllLines(path).Length;
            Assert.Equal(expected,result);
            
            //Clean-up
            File.WriteAllLines(path, lines);
            //Verify that the line is gone
            Assert.Equal(expected-1, File.ReadAllLines(path).Length);
        }
    }
}