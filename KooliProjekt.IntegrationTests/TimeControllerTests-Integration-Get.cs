using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TimesControllerTests_Get : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;

        public TimesControllerTests_Get()
        {
            _client = Factory.CreateClient();
            _dbContext = Factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        private ApplicationDbContext GetDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Times.RemoveRange(dbContext.Times);
            dbContext.SaveChanges();
            return dbContext;
        }

        private async Task<int> CreateTestDoctor()
        {
            using var dbContext = GetDbContext();
            var doctor = new Doctor { Name = "Test", Specialization = "Cardiology" };
            dbContext.Doctors.Add(doctor);
            await dbContext.SaveChangesAsync();
            return doctor.Id;
        }

        private async Task<int> CreateTestTime(int doctorId)
        {
            using var dbContext = GetDbContext();
            var time = new Time
            {
                Date = DateTime.Today,
                VisitTime = new TimeOnly(10, 0),
                Free = true,
                DoctorId = doctorId
            };
            dbContext.Times.Add(time);
            await dbContext.SaveChangesAsync();
            return time.Id;
        }

        [Fact]
        public async Task Index_should_return_times()
        {
            var doctorId = await CreateTestDoctor();
            await CreateTestTime(doctorId);

            var response = await _client.GetAsync("/Times");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("10:00", content);
        }

        [Fact]
        public async Task Details_should_return_notfound_for_missing_id()
        {
            var response = await _client.GetAsync("/Times/Details/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_valid_time()
        {
            var doctorId = await CreateTestDoctor();
            var timeId = await CreateTestTime(doctorId);

            var response = await _client.GetAsync($"/Times/Details/{timeId}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            var response = await _client.GetAsync("/Times");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            var response = await _client.GetAsync("/Time/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            var response = await _client.GetAsync("/Times");
            response.EnsureSuccessStatusCode();
        }

    }
}
