using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class InvoiceLineControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public InvoiceLineControllerTests()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Details_should_return_notfound_when_invoicelines_was_not_found()
        {
            // Act
            using var response = await _client.GetAsync("/InvoiceLines/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Act
            using var response = await _client.GetAsync("/InvoiceLines/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_invoicelines_was_found()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Service = "Dr. Smith", Price = 1 };
            _context.InvoiceLines.Add(invoiceLine);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/InvoiceLines/Details/{invoiceLine.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
