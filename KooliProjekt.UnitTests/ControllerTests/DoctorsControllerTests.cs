using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TodoListsControllerTests
    {
        private readonly Mock<IDoctorService> _DoctorServiceMock;
        private readonly DoctorsController _controller;

        public TodoListsControllerTests()
        {
            _DoctorServiceMock = new Mock<IDoctorService>();
            _controller = new DoctorsController(_DoctorServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Test 1" },
                new Doctor { Id = 2, Name = "Test 2" }
            };
            var pagedResult = new PagedResult<Doctor> { Results = data };
            _DoctorServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}