using Microsoft.Data.Sqlite;

namespace Chirp.Razor
{
    public class DBFacade(string dbPath)
    {
        private readonly string _connectionString = $"Data Source={dbPath};";

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public List<CheepViewModel> ExecuteQuery(string query, string author = "")
        {
            var cheeps = new List<CheepViewModel>();

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;
                if (!string.IsNullOrEmpty(author))
                {
                    command.Parameters.AddWithValue("@Author", author);
                }

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cheeps.Add(new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetInt64(2)));
                }
            }

            return cheeps;
        }
    }
}
