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
    public class InvoiceControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public InvoiceControllerTests()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        

        [Fact]
        public async Task Details_should_return_notfound_when_invoice_was_not_found()
        {
            // Act
            using var response = await _client.GetAsync("/Invoices/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Act
            using var response = await _client.GetAsync("/Invoices/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_invoice_was_found()
        {
            // Arrange
            var invoice = new Invoice { Sum = 1, Paid = true};
            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Invoices/Details/{invoice.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
