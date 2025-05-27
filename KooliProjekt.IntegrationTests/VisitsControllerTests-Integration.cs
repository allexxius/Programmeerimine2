using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class VisitsControllerTests_Post : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public VisitsControllerTests_Post(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            // Get scoped DbContext from factory's services
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Clear Visits before each test run
            _context.Visits.RemoveRange(_context.Visits);
            _context.SaveChanges();
        }

        // Helper method to fetch antiforgery token from /Visits/Create GET
        private async Task<(string Name, string Value)> GetAntiForgeryToken()
        {
            var response = await _client.GetAsync("/Visits/Create");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var tokenName = "__RequestVerificationToken";

            // Simple regex to extract token value from the hidden input field
            var tokenValueMatch = System.Text.RegularExpressions.Regex.Match(html,
                $"<input name=\"{tokenName}\" type=\"hidden\" value=\"([^\"]+)\" />");

            if (!tokenValueMatch.Success)
                throw new HttpRequestException("Antiforgery token not found in HTML.");

            return (tokenName, tokenValueMatch.Groups[1].Value);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_visit()
        {
            // Arrange: get antiforgery token
            var (tokenName, tokenValue) = await GetAntiForgeryToken();

            var formData = new Dictionary<string, string>
            {
                { "Name", "" },      // invalid: empty
                { "Duration", "" },  // invalid: empty
                { tokenName, tokenValue }
            };
            var content = new FormUrlEncodedContent(formData);

            // Act: post create form with invalid data
            var response = await _client.PostAsync("/Visits/Create", content);

            // Assert: Should return OK with validation errors (no redirect)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // No visit should be saved
            var count = await _context.Visits.CountAsync();
            Assert.Equal(0, count);
        }
    }
}
