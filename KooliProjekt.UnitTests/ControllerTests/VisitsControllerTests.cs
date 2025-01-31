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
    public class VisitsControllerTests
    {
        private readonly Mock<IVisitService> _visitServiceMock;
        private readonly VisitsController _controller;

        public VisitsControllerTests()
        {
            _visitServiceMock = new Mock<IVisitService>();
            _controller = new VisitsController(_visitServiceMock.Object);
        }

        // Index Action Tests
        [Fact]
        public async Task Index_ShouldReturnViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<Visit>
            {
                new Visit { Id = 1, Duration = 30 },
                new Visit { Id = 2, Duration = 45 }
            };
            var pagedResult = new PagedResult<Visit> { Results = data };

            _visitServiceMock
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
        public async Task Details_ShouldReturnNotFound_WhenVisitDoesNotExist()
        {
            // Arrange
            int id = 1;
            _visitServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Visit)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithVisit_WhenVisitExists()
        {
            // Arrange
            int id = 1;
            var visit = new Visit { Id = id, Duration = 30 };
            _visitServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(visit);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visit, result.Model);
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
            var visit = new Visit { Id = 1, Duration = 30 };
            _visitServiceMock
                .Setup(x => x.Save(visit))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(visit) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            var visit = new Visit { Id = 1, Duration = 30 };
            _controller.ModelState.AddModelError("Duration", "Required");

            // Act
            var result = await _controller.Create(visit) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visit, result.Model);
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
        public async Task Edit_ShouldReturnNotFound_WhenVisitDoesNotExist()
        {
            // Arrange
            int id = 1;
            _visitServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Visit)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithVisit_WhenVisitExists()
        {
            // Arrange
            int id = 1;
            var visit = new Visit { Id = id, Duration = 30 };
            _visitServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(visit);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visit, result.Model);
        }

        // Edit (POST) Action Tests
        [Fact]
        public async Task Edit_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var visit = new Visit { Id = id, Duration = 30 };
            _visitServiceMock
                .Setup(x => x.Save(visit))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, visit) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var visit = new Visit { Id = id, Duration = 30 };
            _controller.ModelState.AddModelError("Duration", "Required");

            // Act
            var result = await _controller.Edit(id, visit) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visit, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdMismatch()
        {
            // Arrange
            int id = 1;
            var visit = new Visit { Id = 2, Duration = 30 };

            // Act
            var result = await _controller.Edit(id, visit);

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
        public async Task Delete_ShouldReturnNotFound_WhenVisitDoesNotExist()
        {
            // Arrange
            int id = 1;
            _visitServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Visit)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithVisit_WhenVisitExists()
        {
            // Arrange
            int id = 1;
            var visit = new Visit { Id = id, Duration = 30 };
            _visitServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(visit);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visit, result.Model);
        }

        // DeleteConfirmed (POST) Action Test
        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex()
        {
            // Arrange
            int id = 1;
            _visitServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}