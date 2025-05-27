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
    public class DoctorsControllerTests_Get : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public DoctorsControllerTests_Get()
        {
            _client = Factory.CreateClient();
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            var response = await _client.GetAsync("/Doctors");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_doctor_was_not_found()
        {
            var response = await _client.GetAsync("/Doctors/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            var response = await _client.GetAsync("/Doctors/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_doctor_was_found()
        {
            var doctor = new Doctor { Name = "Dr. Smith", Specialization = "Cardiology" };
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            var response = await _client.GetAsync($"/Doctors/Details/{doctor.Id}");
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Doctors/Details")]
        [InlineData("/Doctors/Details/100")]
        [InlineData("/Doctors/Delete")]
        [InlineData("/Doctors/Delete/100")]
        [InlineData("/Doctors/Edit")]
        [InlineData("/Doctors/Edit/100")]
        public async Task Should_return_notfound(string url)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}