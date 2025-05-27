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

        private ApplicationDbContext ResetDatabase()
        {
            var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Invoices.RemoveRange(dbContext.Invoices);
            dbContext.SaveChanges();

            return dbContext;
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
        public async Task Details_should_return_success_when_invoice_exists()
        {
            using var dbContext = ResetDatabase();
            var invoice = new Invoice { Sum = 42, Paid = true };
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
                { "Sum", "100" },
                { "Paid", "true" },
                { "__RequestVerificationToken", await GetAntiForgeryToken(_client, "/Invoices/Create") }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Invoices/Create", content);

            // Debug if needed
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Assert.True(false, $"Form submission failed with BadRequest. Response: {responseBody}");
            }

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("/Invoices", response.Headers.Location.OriginalString);

            var invoice = _dbContext.Invoices.FirstOrDefault();
            Assert.NotNull(invoice);
            Assert.Equal(100, invoice.Sum);
            Assert.True(invoice.Paid);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_invoice()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Sum", "" },
                { "Paid", "" },
                { "__RequestVerificationToken", await GetAntiForgeryToken(_client, "/Invoices/Create") }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Invoices/Create", content);

            // Assert
            // Should return 200 OK when returning to form with validation errors
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using var dbContext = ResetDatabase();
            Assert.False(dbContext.Invoices.Any());
        }

        private async Task<string> GetAntiForgeryToken(HttpClient client, string url)
        {
            // Get the form page
            var getResponse = await client.GetAsync(url);
            getResponse.EnsureSuccessStatusCode();

            // Extract the token from the HTML
            var html = await getResponse.Content.ReadAsStringAsync();
            var startIndex = html.IndexOf("__RequestVerificationToken");
            if (startIndex == -1) return null;

            startIndex = html.IndexOf("value=\"", startIndex) + 7;
            var endIndex = html.IndexOf("\"", startIndex);
            return html.Substring(startIndex, endIndex - startIndex);
        }
    }
}