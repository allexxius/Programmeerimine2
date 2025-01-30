using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TimesControllerTests
    {
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly TimesController _controller;

        public TimesControllerTests()
        {
            _timeServiceMock = new Mock<ITimeService>();
            _controller = new TimesController(_timeServiceMock.Object);
        }

        // Index Action Test
        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<Time>
            {
                new Time { Id = 1, DoctorId = 1 },
                new Time { Id = 2, DoctorId = 2 }
            };
            var pagedResult = new PagedResult<Time> { Results = data };
            _timeServiceMock
                .Setup(x => x.List(page, It.IsAny<int>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }

        // Details Action Tests
        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenTimeDoesNotExist()
        {
            // Arrange
            int id = 1;
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Time)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithTime_WhenTimeExists()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(time);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(time, result.Model);
        }

        // Create (GET) Action Test
        [Fact]
        public void Create_ShouldReturnView()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        // Edit (GET) Action Tests
        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenTimeDoesNotExist()
        {
            // Arrange
            int id = 1;
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Time)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithTime_WhenTimeExists()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(time);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(time, result.Model);
        }

        // Delete (GET) Action Tests
        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenTimeDoesNotExist()
        {
            // Arrange
            int id = 1;
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Time)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithTime_WhenTimeExists()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(time);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(time, result.Model);
        }
    }
}