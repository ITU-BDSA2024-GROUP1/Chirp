using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;
using Chirp.Core.Models;
using Chirp.Core.Repositories;

using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure;

namespace Chirp.InfrastructureTest
{
    public class AuthorServiceUnitTest
    {

        [Fact]
        public static async Task MakeAuthorService()
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=memory");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

            using var context = new ChirpDBContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            IAuthorRepository authorRepository = new AuthorRepository(context);
            AuthorDTO[] knownAuthors = await CheepServiceUnitTest.SetUpTestAuthorDB(authorRepository, connection);

            // Act
            IAuthorService authorService = new AuthorService(authorRepository);

            // Assert
            Assert.NotNull(authorService);
        }


            [Fact]
        public static async Task GetAuthorByName()
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=memory");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

            using var context = new ChirpDBContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            IAuthorRepository authorRepository = new AuthorRepository(context);
            AuthorDTO[] knownAuthors = await CheepServiceUnitTest.SetUpTestAuthorDB(authorRepository, connection);

            IAuthorService authorService = new AuthorService(authorRepository);

            // Act
            AuthorViewModel
                expected = AuthorDTOToAuthorViewModel(knownAuthors[0]),
                actual = await authorService.GetAuthorByName(knownAuthors[0].Name);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static async Task GetAuthorByEmail()
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=memory");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

            using var context = new ChirpDBContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            IAuthorRepository authorRepository = new AuthorRepository(context);
            AuthorDTO[] knownAuthors = await CheepServiceUnitTest.SetUpTestAuthorDB(authorRepository, connection);

            IAuthorService authorService = new AuthorService(authorRepository);

            // Act
            AuthorViewModel
                expected = AuthorDTOToAuthorViewModel(knownAuthors[2]),
                actual = await authorService.GetAuthorByEmail(knownAuthors[2].Email);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static async Task GetAuthorById()
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=memory");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

            using var context = new ChirpDBContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            IAuthorRepository authorRepository = new AuthorRepository(context);
            AuthorDTO[] knownAuthors = await CheepServiceUnitTest.SetUpTestAuthorDB(authorRepository, connection);

            IAuthorService authorService = new AuthorService(authorRepository);

            // Act
            AuthorViewModel
                expected = AuthorDTOToAuthorViewModel(knownAuthors[1]),
                actual = await authorService.GetAuthorById(knownAuthors[1].Id);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static async Task CreateAuthor()
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=memory");
            await connection.OpenAsync();
            var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

            using var context = new ChirpDBContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            IAuthorRepository authorRepository = new AuthorRepository(context);
            AuthorDTO[] knownAuthors = await CheepServiceUnitTest.SetUpTestAuthorDB(authorRepository, connection);

            IAuthorService authorService = new AuthorService(authorRepository);

            // Act
            AuthorViewModel expected = new AuthorViewModel(knownAuthors[knownAuthors.Length - 1].Id + 1, "Test5", "Test5@Tester.com");
            int authorId = await authorService.CreateAuthor(expected);
            AuthorViewModel actual = await authorService.GetAuthorById(authorId);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static AuthorViewModel AuthorDTOToAuthorViewModel(AuthorDTO authorDTO)
        {
            return new AuthorViewModel(authorDTO.Id, authorDTO.Name, authorDTO.Email);
        }

    }
}
