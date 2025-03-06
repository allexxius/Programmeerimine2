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

    public class DoctorsControllerTests : TestBase

    {

        private readonly HttpClient _client;

        public DoctorsControllerTests()

        {

            var options = new WebApplicationFactoryClientOptions

            {

                AllowAutoRedirect = false

            };

            _client = Factory.CreateClient(options);

        }

        private ApplicationDbContext GetDbContext()

        {

            var scope = Factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Doctors.RemoveRange(dbContext.Doctors); // Reset database

            dbContext.SaveChanges();

            return dbContext;

        }

        [Fact]

        public async Task Index_should_return_success()

        {

            using var response = await _client.GetAsync("/Doctors");

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

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_doctor_was_not_found()

        {

            using var response = await _client.GetAsync("/Doctors/Details/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_success_when_doctor_was_found()

        {

            using var dbContext = GetDbContext();

            var doctor = new Doctor { Name = "Test", Specialization = "Cardiology" };

            dbContext.Doctors.Add(doctor);

            dbContext.SaveChanges();

            using var response = await _client.GetAsync($"/Doctors/Details/{doctor.Id}");

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

            if (response.StatusCode == HttpStatusCode.BadRequest)

            {

                var responseBody = await response.Content.ReadAsStringAsync();

                Assert.True(false, $"Form submission failed with BadRequest. Response: {responseBody}");

            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            using var dbContext = GetDbContext();

            var doctor = dbContext.Doctors.FirstOrDefault();

            Assert.NotNull(doctor);

            Assert.Equal("Test Doctor", doctor.Name);

            Assert.Equal("Cardiology", doctor.Specialization);

        }

        [Fact]

        public async Task Create_should_not_save_invalid_new_doctor()

        {

            var formValues = new Dictionary<string, string>

            {

                { "Name", "" },

                { "Specialization", "" }

            };

            using var content = new FormUrlEncodedContent(formValues);

            using var response = await _client.PostAsync("/Doctors/Create", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            using var dbContext = GetDbContext();

            Assert.False(dbContext.Doctors.Any());

        }

    }

}
