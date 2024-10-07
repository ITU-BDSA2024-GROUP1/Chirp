using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace Chirp.RazorTest
{
    public class EndToEndTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;

        public EndToEndTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();  // Create a test HTTP client for Chirp.Razor
        }

        [Fact]
        public async Task GetHomePage_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
