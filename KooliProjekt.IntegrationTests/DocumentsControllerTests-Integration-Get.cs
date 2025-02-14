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
    public class DocumentsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public DocumentsControllerTests()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Act
            using var response = await _client.GetAsync("/Documents");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_doctor_was_not_found()
        {
            // Act
            using var response = await _client.GetAsync("/Documents/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Act
            using var response = await _client.GetAsync("/Documents/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_doctor_was_found()
        {
            // Arrange
            var document = new Document { File = "Dr. Smith", Type = "Cardiology" };
            _context.Documents.Add(document);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync($"/Doctors/Details/{document.ID}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
