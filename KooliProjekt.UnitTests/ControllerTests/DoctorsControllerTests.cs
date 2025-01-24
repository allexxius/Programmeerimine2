using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Models; // Assuming Doctor and PagedResult are defined here
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KooliProjekt.Data;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class DoctorsControllerTests
    {
        private readonly Mock<IDoctorService> _doctorServiceMock;
        private readonly DoctorsController _controller;

        public DoctorsControllerTests()
        {
            _doctorServiceMock = new Mock<IDoctorService>();
            _controller = new DoctorsController(_doctorServiceMock.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Test 1" },
                new Doctor { Id = 2, Name = "Test 2" }
            };
            var pagedResult = new PagedResult<Doctor> { Results = data }; // Assuming PagedResult is still in use
            _doctorServiceMock
                .Setup(x => x.List(page, 10)) // Use List instead of GetPagedAsync
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(pagedResult, result.Model);
        }

    }
}