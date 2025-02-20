using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class DoctorsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public DoctorsControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            // Act
            using var response = await _client.GetAsync("/Doctors");

            // Assert
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
            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_doctor_was_not_found()
        {
            // Act
            using var response = await _client.GetAsync("/Doctors/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_success_when_doctor_was_found()
        {
            // Arrange
            var doctor = new Doctor { Name = "Test", Specialization = "Cardiology" };
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/Doctors/Details/" + doctor.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_new_doctor()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Name", "Test Doctor" },
                { "Specialization", "Cardiology" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Doctors/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var doctor = _context.Doctors.FirstOrDefault();
            Assert.NotNull(doctor);
            Assert.Equal("Test Doctor", doctor.Name);
            Assert.Equal("Cardiology", doctor.Specialization);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_doctor()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Name", "" }, // Invalid: Name is required
                { "Specialization", "" } // Invalid: Specialization is required
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Doctors/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Should return the form with validation errors
            Assert.False(_context.Doctors.Any()); // No doctor should be saved
        }
    }
}