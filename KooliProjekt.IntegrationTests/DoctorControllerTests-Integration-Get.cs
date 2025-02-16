using System.Net;

using System.Net.Http;

using System.Threading.Tasks;

using KooliProjekt.Data;

using KooliProjekt.Models;

using KooliProjekt.Services; // Add this namespace

using KooliProjekt.IntegrationTests.Helpers;

using Xunit;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.IntegrationTests

{

    [Collection("Sequential")]

    public class DoctorControllerTests : TestBase

    {

        private readonly HttpClient _client;

        private readonly ApplicationDbContext _context;

        public DoctorControllerTests()

        {

            _client = Factory.WithWebHostBuilder(builder =>

            {

                builder.ConfigureServices(services =>

                {

                    // Register the DoctorService in the DI container

                    services.AddScoped<IDoctorService, DoctorService>();

                    // Ensure the DbContext is registered with the in-memory database

                    services.AddDbContext<ApplicationDbContext>(options =>

                    {

                        options.UseInMemoryDatabase("TestDatabase");

                    });

                });

            }).CreateClient();

            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));

        }

        [Fact]

        public async Task Index_should_return_correct_response()

        {

            // Act

            using var response = await _client.GetAsync("/Doctors");

            // Assert

            response.EnsureSuccessStatusCode();

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

        public async Task Details_should_return_notfound_when_id_is_missing()

        {

            // Act

            using var response = await _client.GetAsync("/Doctors/Details/");

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]

        public async Task Details_should_return_ok_when_doctor_was_found()

        {

            // Arrange

            var doctor = new Doctor { Name = "Dr. Smith", Specialization = "Cardiology" };

            _context.Doctors.Add(doctor);

            _context.SaveChanges();

            // Act

            using var response = await _client.GetAsync($"/Doctors/Details/{doctor.Id}");

            // Assert

            response.EnsureSuccessStatusCode();

        }

    }

}
