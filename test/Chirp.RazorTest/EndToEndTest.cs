using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Chirp.Core.DataTransferObject;

namespace Chirp.RazorTest
{
    public class EndToEndTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;

        public EndToEndTest(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("RUNNING_TESTS", "true");
            _client = factory.CreateClient();  // Create a test HTTP client for Chirp.Razor
        }

        [Fact]
        public async Task GetHomePage_ReturnsSuccess()
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=memory");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

            using var context = new ChirpDBContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            ICheepRepository cheepRepository = new CheepRepository(context);
            IAuthorRepository authorRepository = new AuthorRepository(context);
            AuthorDTO[] knownAuthors = await SetUpTestAuthorDB(authorRepository, connection);
            CheepDTO[] knownCheeps = await SetUpTestCheepDB(cheepRepository, connection, knownAuthors);

            // Act
            var response = await _client.GetAsync("/");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            
        }

        public static async Task<AuthorDTO[]> SetUpTestAuthorDB(IAuthorRepository authorRepository, SqliteConnection connection)
        {
            using (var command = new SqliteCommand("DELETE FROM authors;", connection))
            {
                command.ExecuteNonQuery();
            }

            AuthorDTO[] authors = new AuthorDTO[4];
            for (int i = 0; i < authors.Length; i++)
            {
                authors[i] = new AuthorDTO
                {
                    Id = i + 1,
                    Name = $"Test{i + 1}",
                    Email = $"Test{i + 1}@Tester.com"
                };
                authors[i].Id = await authorRepository.AddAuthorAsync(authors[i]);
            }

            return authors;

        }
        public static async Task<CheepDTO[]> SetUpTestCheepDB(ICheepRepository cheepRepository, SqliteConnection connection, AuthorDTO[] authors)
        {
            using (var command = new SqliteCommand("DELETE FROM cheeps;", connection))
            {
                command.ExecuteNonQuery();
            }

            DateTime timeStamp = DateTime.Now;
            long timeStampLong = timeStamp.Ticks;
            CheepDTO[] cheeps = new CheepDTO[160];
            for (int i = 0; i < cheeps.Length; i++)
            {
                timeStampLong += 10000000;
                timeStamp = new DateTime(timeStampLong);
                cheeps[i] = new CheepDTO
                {
                    Id = i + 1,
                    Name = authors[i % authors.Length].Name,
                    Message = $"Text{i + 1}",
                    TimeStamp = timeStamp.ToString("yyyy\\-MM\\-dd HH\\:mm\\:ss"),
                    AuthorId = authors[i % authors.Length].Id
                };
                cheeps[i].Id = await cheepRepository.AddCheepAsync(cheeps[i]);
            }

            Array.Reverse(cheeps);
            return cheeps;

        }
    }
}
