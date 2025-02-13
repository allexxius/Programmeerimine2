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
using Microsoft.EntityFrameworkCore;

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

        // Create (POST) Action Tests
        [Fact]
        public async Task Create_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var time = new Time { Id = 1, DoctorId = 1 };
            _timeServiceMock
                .Setup(x => x.Save(time))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(time) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            var time = new Time { Id = 1, DoctorId = 1 };
            _controller.ModelState.AddModelError("DoctorId", "Required");

            // Act
            var result = await _controller.Create(time) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(time, result.Model);
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

        // Edit (POST) Action Tests
        [Fact]
        public async Task Edit_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };
            _timeServiceMock
                .Setup(x => x.Save(time))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, time) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };
            _controller.ModelState.AddModelError("DoctorId", "Required");

            // Act
            var result = await _controller.Edit(id, time) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(time, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdMismatch()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = 2, DoctorId = 1 };

            // Act
            var result = await _controller.Edit(id, time);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldRethrowDbUpdateConcurrencyException_WhenTimeExists()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };

            _timeServiceMock
                .Setup(x => x.Save(time))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(time);

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, time));
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenTimeDoesNotExist_AndConcurrencyExceptionOccurs()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };

            _timeServiceMock
                .Setup(x => x.Save(time))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Time)null);

            // Act
            var result = await _controller.Edit(id, time);

            // Assert
            Assert.IsType<NotFoundResult>(result);
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

        // DeleteConfirmed (POST) Action Test
        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenTimeIsDeleted()
        {
            // Arrange
            int id = 1;
            _timeServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        // TimeExists Helper Method Test
        [Fact]
        public void TimeExists_ShouldReturnTrue_WhenTimeExists()
        {
            // Arrange
            int id = 1;
            var time = new Time { Id = id, DoctorId = 1 };
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(time);

            // Act
            var result = _controller.TimeExists(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TimeExists_ShouldReturnFalse_WhenTimeDoesNotExist()
        {
            // Arrange
            int id = 1;
            _timeServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Time)null);

            // Act
            var result = _controller.TimeExists(id);

            // Assert
            Assert.False(result);
        }
    }
}
