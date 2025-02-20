using System.Net;

using System.Net.Http;

using System.Threading.Tasks;

using KooliProjekt.Data;

using KooliProjekt.Models;

using KooliProjekt.IntegrationTests.Helpers;

using Xunit;

using System;

using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.IntegrationTests

{

    [Collection("Sequential")]

    public class VisitControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _context;

        public VisitControllerTests()

        {

            _client = Factory.CreateClient();

            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }

        [Fact]

        public async Task Index_should_return_correct_response()

        {

            // Act

            using var response = await _client.GetAsync("/Visits");

            // Assert

            response.EnsureSuccessStatusCode();

        }

        [Fact]

        public async Task Details_should_return_notfound_when_visit_was_not_found()

        {

            // Act

            using var response = await _client.GetAsync("/Visits/Details/100");

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_notfound_when_id_is_missing()

        {

            // Act

            using var response = await _client.GetAsync("/Visits/Details/");

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_ok_when_visit_was_found()

        {

            // Arrange

            var user = new IdentityUser { UserName = "testuser", Email = "testuser@example.com" };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var visit = new Visit

            {

                Name = "Dr. Smith",

                Duration = 1,

                UserId = user.Id // Assign the UserId to the visit

            };

            _context.Visits.Add(visit);

            await _context.SaveChangesAsync();

            // Act

            using var response = await _client.GetAsync($"/Visits/Details/{visit.Id}");

            // Assert

            response.EnsureSuccessStatusCode();

        }

    }

}

