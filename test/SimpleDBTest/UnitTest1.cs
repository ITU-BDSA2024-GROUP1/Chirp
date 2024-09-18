using System.IO;
using SimpleDB;
using Chirp.CLI;


namespace SimpleDBTest
{
    public class UnitTest1
    {
        internal record DBtest_Record(String Author, String Message, long Timestamp);
        CSVDatabase<DBtest_Record> testbase;
        [Fact]
        public void SimpleDB_Instantiation()
        {
            //Arrange
            testbase = CSVDatabase<DBtest_Record>.Instance;
            //Assert
            Assert.NotNull(testbase);
        }

        [Fact]
        public void SimpleDB_read()
        {
            //Arrange
            testbase = CSVDatabase<DBtest_Record>.Instance;
            while (Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
            {
                Directory.SetCurrentDirectory(Path.GetFullPath(".."));
            }
            string path = "data/test.csv";
            CSVDatabase<DBtest_Record>.InTestingDatabase = true;
            IEnumerable<DBtest_Record> results =  testbase.Read(1);
            
            //Act
            Assert.NotNull(results);

        }

        [Fact]
        public void SimpleDB_store()
        {
            //Arrange
            testbase = CSVDatabase<DBtest_Record>.Instance;
            while (Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
            {
                Directory.SetCurrentDirectory(Path.GetFullPath(".."));
            }
            string path = "data/test.csv";
            String[] lines = File.ReadAllLines(path);
            int expected = lines.Length+1 ;
            CSVDatabase<DBtest_Record>.InTestingDatabase = true;
            testbase.Store(new DBtest_Record( "AuthorDB", "DBmessage", 0));
            
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