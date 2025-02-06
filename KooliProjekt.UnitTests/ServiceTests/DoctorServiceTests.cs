using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using KooliProjekt.Data;

using KooliProjekt.Models;

using KooliProjekt.Search;

using KooliProjekt.Services;

using Microsoft.AspNetCore.Routing;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace KooliProjekt.Tests.Services

{

    [TestFixture]

    public class DoctorServiceTests

    {

        private Mock<ApplicationDbContext> _mockContext;

        private DoctorService _doctorService;

        private Mock<DbSet<Doctor>> _mockSet;

        [SetUp]

        public void SetUp()

        {

            _mockContext = new Mock<ApplicationDbContext>();

            _mockSet = new Mock<DbSet<Doctor>>();

            _doctorService = new DoctorService(_mockContext.Object);

        }

        [Test]

        public async Task List_WithoutSearchParameters_ReturnsPagedResult()

        {

            // Arrange

            var data = new List<Doctor>

            {

                new Doctor { Id = 1, Name = "Dr. Smith" },

                new Doctor { Id = 2, Name = "Dr. Johnson" }

            }.AsQueryable();

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(data.Provider);

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(data.Expression);

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(data.ElementType);

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(c => c.Doctors).Returns(_mockSet.Object);

            // Act

            var result = await _doctorService.List(1, 10);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Results.Count());

        }

        [Test]

        public async Task List_WithSearchParameters_ReturnsFilteredPagedResult()

        {

            // Arrange

            var data = new List<Doctor>

            {

                new Doctor { Id = 1, Name = "Dr. Smith" },

                new Doctor { Id = 2, Name = "Dr. Johnson" }

            }.AsQueryable();

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(data.Provider);

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(data.Expression);

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(data.ElementType);

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(c => c.Doctors).Returns(_mockSet.Object);

            var search = new DoctorSearch { Keyword = "Smith" };

            // Act

            var result = await _doctorService.List(1, 10, search);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.Results.Count());

            Assert.AreEqual("Dr. Smith", result.Results.First().Name);

        }

        [Test]

        public async Task Get_ReturnsDoctorById()

        {

            // Arrange

            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };

            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(doctor);

            _mockContext.Setup(c => c.Doctors).Returns(_mockSet.Object);

            // Act

            var result = await _doctorService.Get(1);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual("Dr. Smith", result.Name);

        }

        [Test]

        public async Task Save_AddsNewDoctor()

        {

            // Arrange

            var doctor = new Doctor { Id = 0, Name = "Dr. Smith" };

            _mockSet.Setup(m => m.Add(It.IsAny<Doctor>())).Callback<Doctor>(d => d.Id = 1);

            _mockContext.Setup(c => c.Doctors).Returns(_mockSet.Object);

            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act

            await _doctorService.Save(doctor);

            // Assert

            _mockSet.Verify(m => m.Add(It.IsAny<Doctor>()), Times.Once);

            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            Assert.AreEqual(1, doctor.Id);

        }

        [Test]

        public async Task Save_UpdatesExistingDoctor()

        {

            // Arrange

            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };

            _mockSet.Setup(m => m.Update(It.IsAny<Doctor>()));

            _mockContext.Setup(c => c.Doctors).Returns(_mockSet.Object);

            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act

            await _doctorService.Save(doctor);

            // Assert

            _mockSet.Verify(m => m.Update(It.IsAny<Doctor>()), Times.Once);

            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

        }

        [Test]

        public async Task Delete_RemovesDoctor()

        {

            // Arrange

            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };


            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(doctor);

            _mockContext.Setup(c => c.Doctors).Returns(_mockSet.Object);

            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act

            await _doctorService.Delete(1);

            // Assert

            _mockSet.Verify(m => m.Remove(It.IsAny<Doctor>()), Times.Once);

            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

        }

    }

}
