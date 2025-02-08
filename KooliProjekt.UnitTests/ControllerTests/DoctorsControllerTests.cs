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
using Microsoft.EntityFrameworkCore;

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
            int page = 1;
            var data = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Test 1" },
                new Doctor { Id = 2, Name = "Test 2" }
            };
            var pagedResult = new PagedResult<Doctor> { Results = data, CurrentPage = page, PageSize = 10, RowCount = data.Count, PageCount = 1 };

            _doctorServiceMock
                .Setup(x => x.List(page, 10, null))
                .ReturnsAsync(pagedResult);

            var result = await _controller.Index(page) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as DoctorIndexModel;
            Assert.NotNull(model.Data);
            Assert.Equal(pagedResult.Results, model.Data.Results);
            Assert.Equal(pagedResult.CurrentPage, model.Data.CurrentPage);
            Assert.Equal(pagedResult.PageSize, model.Data.PageSize);
            Assert.Equal(pagedResult.RowCount, model.Data.RowCount);
            Assert.Equal(pagedResult.PageCount, model.Data.PageCount);
        }

        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithSearchData()
        {
            int page = 1;
            var search = new DoctorSearch { Keyword = "Test" };
            var data = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Test Doctor 1" },
                new Doctor { Id = 2, Name = "Test Doctor 2" }
            };
            var pagedResult = new PagedResult<Doctor> { Results = data, CurrentPage = page, PageSize = 10, RowCount = data.Count, PageCount = 1 };

            _doctorServiceMock
                .Setup(x => x.List(page, 10, search))
                .ReturnsAsync(pagedResult);

            var result = await _controller.Index(page, search) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = result.Model as DoctorIndexModel;
            Assert.NotNull(model.Data);
            Assert.Equal(pagedResult.Results, model.Data.Results);
            Assert.Equal(pagedResult.CurrentPage, model.Data.CurrentPage);
            Assert.Equal(pagedResult.PageSize, model.Data.PageSize);
            Assert.Equal(pagedResult.RowCount, model.Data.RowCount);
            Assert.Equal(pagedResult.PageCount, model.Data.PageCount);
            Assert.Equal(search, model.Search);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenIdIsNull()
        {
            int? id = null;

            var result = await _controller.Details(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            var result = await _controller.Details(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithDoctor_WhenDoctorExists()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            var result = await _controller.Details(id) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public void Create_ShouldReturnView()
        {
            var result = _controller.Create() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
        {
            var doctor = new Doctor { Id = 1, Name = "Test Doctor" };
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(doctor) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public async Task Create_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            var doctor = new Doctor { Id = 1, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Save(doctor))
                .Returns(Task.CompletedTask);

            var result = await _controller.Create(doctor) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _doctorServiceMock.Verify(x => x.Save(doctor), Times.Once);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdIsNull()
        {
            int? id = null;

            var result = await _controller.Edit(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            var result = await _controller.Edit(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithDoctor_WhenDoctorExists()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            var result = await _controller.Edit(id) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdDoesNotMatch()
        {
            int id = 1;
            var doctor = new Doctor { Id = 2, Name = "Test Doctor" };

            var result = await _controller.Edit(id, doctor);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Edit(id, doctor) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Save(doctor))
                .Returns(Task.CompletedTask);

            var result = await _controller.Edit(id, doctor) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _doctorServiceMock.Verify(x => x.Save(doctor), Times.Once);
        }

        [Fact]
        public async Task Edit_ShouldThrowDbUpdateConcurrencyException_WhenDoctorDoesNotExist()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Save(doctor))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, doctor));
        }

        [Fact]
        public async Task Edit_ShouldRethrowDbUpdateConcurrencyException_WhenDoctorExists()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Save(doctor))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, doctor));
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenIdIsNull()
        {
            int? id = null;

            var result = await _controller.Delete(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            var result = await _controller.Delete(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithDoctor_WhenDoctorExists()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            var result = await _controller.Delete(id) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(doctor, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldCallDeleteMethod_AndRedirectToIndex()
        {
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _doctorServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        [Fact]
        public void DoctorExists_ShouldReturnTrue_WhenDoctorExists()
        {
            int id = 1;
            var doctor = new Doctor { Id = id, Name = "Test Doctor" };

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(doctor);

            var result = _controller.DoctorExists(id);

            Assert.True(result);
        }

        [Fact]
        public void DoctorExists_ShouldReturnFalse_WhenDoctorDoesNotExist()
        {
            int id = 1;

            _doctorServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Doctor)null);

            var result = _controller.DoctorExists(id);

            Assert.False(result);
        }
    }
}