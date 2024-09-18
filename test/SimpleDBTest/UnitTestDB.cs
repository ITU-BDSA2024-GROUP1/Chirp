using System.IO;
using SimpleDB;
using Chirp.CLI;


namespace SimpleDBTest
{
    [Collection("Non-Parallel Collection")]
    public class UnitTestDB
    {
        const string Path = "data/test.csv";
        private CSVDatabase<DBTestRecord>? _testBase;
        
        private record DBTestRecord(String Author, String Message, long Timestamp);
        
        [Fact]
        public void SimpleDB_Instantiation()
        {
            //Arrange
            _testBase = CSVDatabase<DBTestRecord>.Instance;
            //Assert
            Assert.NotNull(_testBase);
        }

        [Fact]
        public void SimpleDB_read()
        {
            //Arrange
            _testBase = CSVDatabase<DBTestRecord>.Instance;
            while (System.IO.Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
            {
                Directory.SetCurrentDirectory(System.IO.Path.GetFullPath(".."));
            }

            CSVDatabase<DBTestRecord>.InTestingDatabase = true;
            IEnumerable<DBTestRecord> results =  _testBase.Read(1);
            
            //Act
            Assert.NotNull(results);
        }

        [Fact]
        public void SimpleDB_store()
        {
            //Arrange
            _testBase = CSVDatabase<DBTestRecord>.Instance;
            Program.SetWorkingDirectoryToProjectRoot();
            
            String[] lines = File.ReadAllLines(Path);
            int expected = lines.Length + 1;
            CSVDatabase<DBTestRecord>.InTestingDatabase = true;
            _testBase.Store(new DBTestRecord( "AuthorDB", "DBMessage", 0));
            
            //Act
            int result = File.ReadAllLines(Path).Length;
            Assert.Equal(expected,result);
            
            //Clean-up
            File.WriteAllLines(Path, lines);
            
            //Verify that the line is gone
            Assert.Equal(expected - 1, File.ReadAllLines(Path).Length);
        }
    }
}