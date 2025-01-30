using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KooliProjekt.Data;
using KooliProjekt.Search;

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

        // Index Action Tests
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
            var pagedResult = new PagedResult<Doctor> { Results = data };

            _doctorServiceMock
                .Setup(x => x.List(page, 10))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as DoctorIndexModel;
            Assert.Equal(pagedResult, model.Data);
        }

        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithSearchData()
        {
            // Arrange
            int page = 1;
            var search = new DoctorSearch { Keyword = "Test" };
            var data = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Test Doctor 1" },
                new Doctor { Id = 2, Name = "Test Doctor 2" }
            };
            var pagedResult = new PagedResult<Doctor> { Results = data };

            _doctorServiceMock
                .Setup(x => x.List(page, 10, search))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, search) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as DoctorIndexModel;
            Assert.Equal(pagedResult, model.Data);
            Assert.Equal(search, model.Search);
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
        public async Task Details_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithDoctor_WhenDoctorExists()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        // Create (GET) Action Tests
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
        public async Task Create_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var doctor = new Doctor { Id = 1, Name = "Test Doctor" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(doctor) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public async Task Create_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var doctor = new Doctor { Id = 1, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Save(doctor))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(doctor) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _doctorServiceMock.Verify(x => x.Save(doctor), Times.Once);
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
        public async Task Edit_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithDoctor_WhenDoctorExists()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        // Edit (POST) Action Tests
        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdDoesNotMatch()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = 2, Name = "Test Doctor" };

            // Act
            var result = await _controller.Edit(id, doctor);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithDoctor_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(id, doctor) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Save(doctor))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, doctor) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _doctorServiceMock.Verify(x => x.Save(doctor), Times.Once);
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
        public async Task Delete_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithDoctor_WhenDoctorExists()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        // DeleteConfirmed (POST) Action Tests
        [Fact]
        public async Task DeleteConfirmed_ShouldCallDeleteMethod_AndRedirectToIndex()
        {
            // Arrange
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _doctorServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        // DoctorExists Method Tests (Kaudselt testitud läbi Edit ja Delete meetodite)
        [Fact]
        public async Task DoctorExists_ShouldReturnTrue_WhenDoctorExists()
        {
            // Arrange
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result); // Kui DoctorExists tagastab true, peaks Edit meetod tagastama vaate
        }

        [Fact]
        public async Task DoctorExists_ShouldReturnFalse_WhenDoctorDoesNotExist()
        {
            // Arrange
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result); // Kui DoctorExists tagastab false, peaks Edit meetod tagastama NotFound
        }
    }
}