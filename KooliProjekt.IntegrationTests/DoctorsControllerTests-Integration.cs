using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class DoctorsControllerTests_Post : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;

        public DoctorsControllerTests_Post()
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
            _dbContext = Factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        private ApplicationDbContext GetDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Doctors.RemoveRange(dbContext.Doctors);
            dbContext.SaveChanges();
            return dbContext;
        }

        [Fact]
        public async Task Create_should_save_new_doctor()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Name", "Test Doctor" },
                { "Specialization", "Cardiology" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            var response = await _client.PostAsync("/Doctors/Create", content);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Form submission failed with BadRequest. Response: {responseBody}");
            }

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var doctor = _dbContext.Doctors.FirstOrDefault();
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
                { "Userid", "" },
                { "Specialization", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            var response = await _client.PostAsync("/Doctors/Create", content);
            response.EnsureSuccessStatusCode();

            using var dbContext = GetDbContext();
            Assert.False(dbContext.Doctors.Any());
        }
    }
}
