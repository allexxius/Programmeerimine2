using System.Net;

using System.Net.Http;

using System.Threading.Tasks;

using KooliProjekt.Data;

using KooliProjekt.Models;

using KooliProjekt.IntegrationTests.Helpers;

using Xunit;

using System;

namespace KooliProjekt.IntegrationTests

{

    [Collection("Sequential")]

    public class TimesControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _context;

        public TimesControllerTests()

        {

            _client = Factory.CreateClient();

            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }

        [Fact]

        public async Task Index_should_return_correct_response()

        {

            // Act

            using var response = await _client.GetAsync("/Times");

            // Assert

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Details_should_return_notfound_when_times_was_not_found()

        {

            // Act

            using var response = await _client.GetAsync("/Time/Details/100");

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_id_is_missing()

        {

            // Act

            using var response = await _client.GetAsync("/Time/Details/");

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_ok_when_time_was_found()

        {

            // Arrange

            var time = new Time { Free = true, Date = DateTime.Now };

            _context.Times.Add(time);

            _context.SaveChanges();

            // Act

            using var response = await _client.GetAsync($"/Times/Details/{time.Id}");

            // Assert

            response.EnsureSuccessStatusCode();

        }

    }

}

