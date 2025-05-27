using System;
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
    public class TimesControllerTests_Post : TestBase
    {
        private readonly HttpClient _client;

        public TimesControllerTests_Post()
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
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
        public async Task Create_should_add_new_time()
        {
            var doctorId = await CreateTestDoctor();
            var now = DateTime.Now;

            var getResponse = await _client.GetAsync("/Times/Create");
            var body = await getResponse.Content.ReadAsStringAsync();
            var token = GetAntiForgeryToken(body);

            var form = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "Date", now.ToString("yyyy-MM-dd") },
                { "VisitTime", $"{now.Hour}:{now.Minute}" },
                { "Free", "true" },
                { "DoctorId", doctorId.ToString() }
            };

            var response = await _client.PostAsync("/Times/Create", new FormUrlEncodedContent(form));
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Times", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Edit_should_update_existing_time()
        {
            var doctorId = await CreateTestDoctor();
            var timeId = await CreateTestTime(doctorId);
            var newDate = DateTime.Today.AddDays(1);

            var getResponse = await _client.GetAsync($"/Times/Edit/{timeId}");
            var body = await getResponse.Content.ReadAsStringAsync();
            var token = GetAntiForgeryToken(body);

            var form = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "Id", timeId.ToString() },
                { "Date", newDate.ToString("yyyy-MM-dd") },
                { "VisitTime", $"{newDate.Hour}:{newDate.Minute}" },
                { "Free", "false" },
                { "DoctorId", doctorId.ToString() }
            };

            var response = await _client.PostAsync($"/Times/Edit/{timeId}", new FormUrlEncodedContent(form));
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_time()
        {
            var doctorId = await CreateTestDoctor();
            var timeId = await CreateTestTime(doctorId);

            var getResponse = await _client.GetAsync($"/Times/Delete/{timeId}");
            var body = await getResponse.Content.ReadAsStringAsync();
            var token = GetAntiForgeryToken(body);

            var form = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "id", timeId.ToString() }
            };

            var response = await _client.PostAsync($"/Times/Delete/{timeId}", new FormUrlEncodedContent(form));
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        private string GetAntiForgeryToken(string html)
        {
            var pattern = @"<input[^>]*name=""__RequestVerificationToken""[^>]*value=""([^""]*)""";
            var match = System.Text.RegularExpressions.Regex.Match(html, pattern);
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
