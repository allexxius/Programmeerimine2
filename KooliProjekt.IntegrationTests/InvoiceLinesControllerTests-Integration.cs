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

    public class InvoiceLinesControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _dbContext;

        public InvoiceLinesControllerTests()

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

            dbContext.InvoiceLines.RemoveRange(dbContext.InvoiceLines); // Reset database

            dbContext.SaveChanges();

            return dbContext;

        }

        [Fact]

        public async Task Index_should_return_success()

        {

            using var response = await _client.GetAsync("/InvoiceLines");

            response.EnsureSuccessStatusCode();

        }

        [Theory]

        [InlineData("/InvoiceLines/Details")]

        [InlineData("/InvoiceLines/Details/100")]

        [InlineData("/InvoiceLines/Delete")]

        [InlineData("/InvoiceLines/Delete/100")]

        [InlineData("/InvoiceLines/Edit")]

        [InlineData("/InvoiceLines/Edit/100")]

        public async Task Should_return_notfound(string url)

        {

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_invoiceline_was_not_found()

        {

            using var response = await _client.GetAsync("/InvoiceLines/Details/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_success_when_invoiceline_was_found()

        {

            using var dbContext = GetDbContext();

            var invoiceLine = new InvoiceLine { Service = "Test", Price = 1 };

            dbContext.InvoiceLines.Add(invoiceLine);

            dbContext.SaveChanges();

            using var response = await _client.GetAsync($"/InvoiceLines/Details/{invoiceLine.Id}");

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Create_should_save_new_invoiceline()

        {

            // Arrange

            var formValues = new Dictionary<string, string>

            {
                { "Service", "Test" },

                { "Price", "1" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act

            using var response = await _client.PostAsync("/InvoiceLines/Create", content);

            // Assert

            if (response.StatusCode == HttpStatusCode.BadRequest)

            {

                var responseBody = await response.Content.ReadAsStringAsync();

                Assert.Fail($"Form submission failed with BadRequest. Response: {responseBody}");

            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var invoiceLine = _dbContext.InvoiceLines.FirstOrDefault();

            Assert.NotNull(invoiceLine);

            Assert.Equal("Test", invoiceLine.Service);

            Assert.Equal(1, invoiceLine.Price);

        }

        [Fact]

        public async Task Create_should_not_save_invalid_new_invoiceline()

        {

            var formValues = new Dictionary<string, string>

            {

                { "Service", "" },

                { "Price", "" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            using var response = await _client.PostAsync("/InvoiceLines/Create", content);

            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();

            Assert.False(dbContext.InvoiceLines.Any());

        }

    }

}

