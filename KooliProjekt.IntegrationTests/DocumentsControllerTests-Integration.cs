using System.Collections.Generic;

using System.Linq;

using System.Net;

using System.Net.Http;

using System.Threading.Tasks;

using KooliProjekt.Data;

using KooliProjekt.IntegrationTests.Helpers;

using KooliProjekt.Models;

using Microsoft.AspNetCore.Mvc.Testing;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace KooliProjekt.IntegrationTests

{

    [Collection("Sequential")]

    public class DocumentsControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _dbContext;

        public DocumentsControllerTests()

        {

            var options = new WebApplicationFactoryClientOptions

            {

                AllowAutoRedirect = false

            };

            _client = Factory.CreateClient(options);

            _dbContext = Factory.Services.GetRequiredService<ApplicationDbContext>();

        }

        private ApplicationDbContext GetDbContext()

        {

            var scope = Factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Documents.RemoveRange(dbContext.Documents); // Reset database

            dbContext.SaveChanges();

            return dbContext;

        }

        [Fact]

        public async Task Index_should_return_success()

        {

            using var response = await _client.GetAsync("/Documents");

            response.EnsureSuccessStatusCode();

        }

        [Theory]

        [InlineData("/Documents/Details")]

        [InlineData("/Documents/Details/100")]

        [InlineData("/Documents/Delete")]

        [InlineData("/Documents/Delete/100")]

        [InlineData("/Documents/Edit")]

        [InlineData("/Documents/Edit/100")]

        public async Task Should_return_notfound(string url)

        {

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_document_was_not_found()

        {

            using var response = await _client.GetAsync("/Documents/Details/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_success_when_document_was_found()

        {

            using var dbContext = GetDbContext();

            var document = new Document { Type = "Test", File = "Test" };

            dbContext.Documents.Add(document);

            dbContext.SaveChanges();

            using var response = await _client.GetAsync($"/Documents/Details/{document.Id}");

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Create_should_save_new_document()

        {

            // Arrange

            var formValues = new Dictionary<string, string>

            {

                { "Type", "Test" },

                { "File", "Test" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act

            using var response = await _client.PostAsync("/Documents/Create", content);

            // Assert

            if (response.StatusCode == HttpStatusCode.BadRequest)

            {

                var responseBody = await response.Content.ReadAsStringAsync();

                Assert.Fail($"Form submission failed with BadRequest. Response: {responseBody}");

            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var document = _dbContext.Documents.FirstOrDefault();

            Assert.NotNull(document);

            Assert.Equal("Test", document.Type);

            Assert.Equal("Test", document.File);

        }

        [Fact]

        public async Task Create_should_not_save_invalid_new_document()

        {

            var formValues = new Dictionary<string, string>

            {

                { "Name", "" },

                { "File", "" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            using var response = await _client.PostAsync("/Documents/Create", content);

            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();

            Assert.False(dbContext.Documents.Any());

        }

    }

}

