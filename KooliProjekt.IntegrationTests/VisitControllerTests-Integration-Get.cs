using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class VisitsControllerTests_Get : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public VisitsControllerTests_Get(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Clean visits before each test
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Visits.RemoveRange(db.Visits);
            db.SaveChanges();
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            var response = await _client.GetAsync("/Visits");
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Visits/Details")]
        [InlineData("/Visits/Details/100")]
        [InlineData("/Visits/Delete")]
        [InlineData("/Visits/Delete/100")]
        [InlineData("/Visits/Edit")]
        [InlineData("/Visits/Edit/100")]
        public async Task Should_return_notfound_for_missing_or_invalid_id(string url)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_success_when_visit_exists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // We need to create a user as UserId is required (non-null)
            var user = new Microsoft.AspNetCore.Identity.IdentityUser { UserName = "testuser", Email = "testuser@example.com" };
            db.Users.Add(user);
            db.SaveChanges();

            var visit = new Visit
            {
                Name = "Test Visit",
                Duration = 1,
                UserId = user.Id
            };
            db.Visits.Add(visit);
            db.SaveChanges();

            var response = await _client.GetAsync($"/Visits/Details/{visit.Id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
