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

    public class VisitsControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _dbContext;

        public VisitsControllerTests()

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

            dbContext.Visits.RemoveRange(dbContext.Visits); // Reset database

            dbContext.SaveChanges();

            return dbContext;

        }

        [Fact]

        public async Task Index_should_return_success()

        {

            using var response = await _client.GetAsync("/Visits");

            response.EnsureSuccessStatusCode();

        }

        [Theory]

        [InlineData("/Visits/Details")]

        [InlineData("/Visits/Details/100")]

        [InlineData("/Visits/Delete")]

        [InlineData("/Visits/Delete/100")]

        [InlineData("/Visits/Edit")]

        [InlineData("/Visits/Edit/100")]

        public async Task Should_return_notfound(string url)

        {

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_visit_was_not_found()

        {

            using var response = await _client.GetAsync("/Visits/Details/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_success_when_visit_was_found()

        {

            using var dbContext = GetDbContext();

            var visit = new Visit { Name = "Test", Duration = 1 };

            dbContext.Visits.Add(visit);

            dbContext.SaveChanges();

            using var response = await _client.GetAsync($"/Visits/Details/{visit.Id}");

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Create_should_save_new_visit()

        {

            // Arrange

            var formValues = new Dictionary<string, string>

            {

                { "Name", "Test" },

                { "Duration", "1" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act

            using var response = await _client.PostAsync("/Visits/Create", content);

            // Assert

            if (response.StatusCode == HttpStatusCode.BadRequest)

            {

                var responseBody = await response.Content.ReadAsStringAsync();

                Assert.Fail($"Form submission failed with BadRequest. Response: {responseBody}");

            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var visit = _dbContext.Visits.FirstOrDefault();

            Assert.NotNull(visit);

            Assert.Equal("Test", visit.Name);

            Assert.Equal(1, visit.Duration);

        }

        [Fact]

        public async Task Create_should_not_save_invalid_new_visit()

        {

            var formValues = new Dictionary<string, string>

            {

                { "Name", "" },

                { "Duration", "" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            using var response = await _client.PostAsync("/Visits/Create", content);

            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();

            Assert.False(dbContext.Visits.Any());

        }

    }

}

