using System.Buffers;

using Microsoft.Data.Sqlite;
/*
namespace Chirp.RazorTest
{
    public class DBFacadeUnitTest
    {
        [Fact]
        public void MakeDBFacade()
        {
            // Arrange
            string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");
            
            // Act
            DBFacade dbFacade = new DBFacade(dbPath);
            
            // Assert
            Assert.NotNull(dbFacade);

        }

        [Fact]
        public void EstablishConnection()
        {
            // Arrange
            string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");
            DBFacade dbFacade = new DBFacade(dbPath);
            string dbName = "chirp.db";
            
            // Act
            SqliteConnection sc = dbFacade.GetConnection();
            string[] filePath = sc.DataSource.Split("\\");

            // Assert
            Assert.Equal(dbName, filePath[filePath.Length-1]);

        }

        [Fact]
        public void ExecuteQuery()
        {
            // Arrange
            string dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chirp.db");
            DBFacade dbFacade = new DBFacade(dbPath);
            CheepViewModel expected = new CheepViewModel("test", "test", 1000);
            using var connection = new SqliteConnection($"Data Source={dbPath};");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                create table if not exists test (
                  username string primary key,
                  text string not null,
                  pub_date integer
                );
            ";
            command.ExecuteNonQuery();
            command.CommandText = @"
                INSERT INTO test (username, text, pub_date)
                VALUES ('test', 'test', 1000);
            ";
            command.ExecuteNonQuery();

            // Act
            string query = @"
                SELECT t.username, t.text, t.pub_date 
                FROM test t";
            List<CheepViewModel> test = dbFacade.ExecuteQuery(query);

            // Clean up
            command.CommandText = @"
                DROP TABLE test;
            ";
            command.ExecuteNonQuery();


            // Assert
            Assert.Equal(expected, test[test.Count() - 1]);

        }
    }
}
*/