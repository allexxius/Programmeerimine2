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

    public class InvoicesControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _dbContext;

        public InvoicesControllerTests()

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

            dbContext.Invoices.RemoveRange(dbContext.Invoices); // Reset database

            dbContext.SaveChanges();

            return dbContext;

        }

        [Fact]

        public async Task Index_should_return_success()

        {

            using var response = await _client.GetAsync("/Invoices");

            response.EnsureSuccessStatusCode();

        }

        [Theory]

        [InlineData("/Invoices/Details")]

        [InlineData("/Invoices/Details/100")]

        [InlineData("/Invoices/Delete")]

        [InlineData("/Invoices/Delete/100")]

        [InlineData("/Invoices/Edit")]

        [InlineData("/Invoices/Edit/100")]

        public async Task Should_return_notfound(string url)

        {

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_invoice_was_not_found()

        {

            using var response = await _client.GetAsync("/Invoices/Details/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_success_when_invoice_was_found()

        {

            using var dbContext = GetDbContext();

            var invoice = new Invoice { Sum = 1, Paid = true };

            dbContext.Invoices.Add(invoice);

            dbContext.SaveChanges();

            using var response = await _client.GetAsync($"/Invoices/Details/{invoice.Id}");

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Create_should_save_new_invoice()

        {

            // Arrange

            var formValues = new Dictionary<string, string>

            {

                { "Sum", "1" },

                { "Paid", "true" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act

            using var response = await _client.PostAsync("/Invoices/Create", content);

            // Assert

            if (response.StatusCode == HttpStatusCode.BadRequest)

            {

                var responseBody = await response.Content.ReadAsStringAsync();

                Assert.Fail($"Form submission failed with BadRequest. Response: {responseBody}");

            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var invoice = _dbContext.Invoices.FirstOrDefault();

            Assert.NotNull(invoice);

            Assert.Equal(1, invoice.Sum);

            Assert.Equal(true, invoice.Paid);

        }

        [Fact]

        public async Task Create_should_not_save_invalid_new_invoice()

        {

            var formValues = new Dictionary<string, string>

            {

                { "Sum", "" },

                { "Paid", "" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            using var response = await _client.PostAsync("/Invoices/Create", content);

            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();

            Assert.False(dbContext.Invoices.Any());

        }

    }

}

