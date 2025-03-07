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

    public class TimesControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _dbContext;

        public TimesControllerTests()

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

            dbContext.Times.RemoveRange(dbContext.Times); // Reset database

            dbContext.SaveChanges();

            return dbContext;

        }

        [Fact]

        public async Task Index_should_return_success()

        {

            using var response = await _client.GetAsync("/Times");

            response.EnsureSuccessStatusCode();

        }

        [Theory]

        [InlineData("/Times/Details")]

        [InlineData("/Times/Details/100")]

        [InlineData("/Times/Delete")]

        [InlineData("/Times/100")]

        [InlineData("/Times/Edit")]

        [InlineData("/Times/Edit/100")]

        public async Task Should_return_notfound(string url)

        {

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_time_was_not_found()

        {

            using var response = await _client.GetAsync("/Times/Details/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_success_when_time_was_found()

        {

            using var dbContext = GetDbContext();

            var time = new Time { Free = true };

            dbContext.Times.Add(time);

            dbContext.SaveChanges();

            using var response = await _client.GetAsync($"/Times/Details/{time.Id}");

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Create_should_save_new_time()

        {

            // Arrange

            var formValues = new Dictionary<string, string>

            {

                { "Free", "true" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act

            using var response = await _client.PostAsync("/Times/Create", content);

            // Assert

            if (response.StatusCode == HttpStatusCode.BadRequest)

            {

                var responseBody = await response.Content.ReadAsStringAsync();

                Assert.Fail($"Form submission failed with BadRequest. Response: {responseBody}");

            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var time = _dbContext.Times.FirstOrDefault();

            Assert.NotNull(time);

            Assert.Equal(true, time.Free);

        }

        [Fact]

        public async Task Create_should_not_save_invalid_new_time()

        {

            var formValues = new Dictionary<string, string>

            {

                { "Free", "" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            using var response = await _client.PostAsync("/Times/Create", content);

            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();

            Assert.False(dbContext.Times.Any());

        }

    }

}

