using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
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
            // Arrange

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
            // Arrange

            // Act
            using var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_was_not_found()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Doctors/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]

        public async Task Details_should_return_success_when_list_was_found()
        {
            // Arrange
            var list = new Doctor { Specialization = "Test" };
            _context.Doctors.Add(list);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/Doctors/Details/" + list.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]

        public async Task Create_should_save_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            formValues.Add("Id", "0");
            formValues.Add("Name", "Test");

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Doctors/Create", content);

            // Log the actual response code
            Console.WriteLine($"Response Status Code: {response.StatusCode}");

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
               
                 
            var list = _context.Doctors.FirstOrDefault();
            Assert.NotNull(list);
            Assert.NotEqual(0, list.Id);
            Assert.Equal("Test", list.Name);
        }


        [Fact]
        public async Task Create_should_not_save_invalid_new_list()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            formValues.Add("Name", "Test");

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Doctors/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.False(_context.Doctors.Any());
        }
    }
}