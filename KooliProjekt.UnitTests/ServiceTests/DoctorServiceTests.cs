using KooliProjekt.Data;

using KooliProjekt.Data.Repositories;

using KooliProjekt.Services;

using KooliProjekt.Search;

using Moq;

using System.Threading.Tasks;

using Xunit;

namespace KooliProjekt.Tests.Services

{

    public class DoctorServiceTests

    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;

        private readonly DoctorService _doctorService;

        public DoctorServiceTests()

        {

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _doctorRepositoryMock = new Mock<IDoctorRepository>();

            _doctorService = new DoctorService(_unitOfWorkMock.Object, _doctorRepositoryMock.Object);

        }

        [Fact]

        public async Task List_ShouldReturnPagedResult()

        {

            // Arrange

            var page = 1;

            var pageSize = 10;
            var pagedResult = new PagedResult<Doctor>();
            _doctorRepositoryMock.Setup(repo => repo.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act

            var result = await _doctorService.List(page, pageSize, null);

            // Assert

            Assert.Equal(pagedResult, result);

        }

        [Fact]

        public async Task Get_ShouldReturnDoctor()

        {

            // Arrange

            var doctorId = 1;

            var doctor = new Doctor { Id = doctorId };

            _doctorRepositoryMock.Setup(repo => repo.Get(doctorId)).ReturnsAsync(doctor);

            // Act

            var result = await _doctorService.Get(doctorId);

            // Assert

            Assert.Equal(doctor, result);

        }

        [Fact]

        public async Task Save_ShouldCallRepositoryAndCommit()

        {

            // Arrange

            var doctor = new Doctor();

            _doctorRepositoryMock.Setup(repo => repo.Save(doctor)).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act

            await _doctorService.Save(doctor);

            // Assert

            _doctorRepositoryMock.Verify(repo => repo.Save(doctor), Times.Once);

            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);

        }

        [Fact]

        public async Task Delete_ShouldCallRepositoryAndCommit()

        {

            // Arrange

            var doctorId = 1;

            _doctorRepositoryMock.Setup(repo => repo.Delete(doctorId)).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act

            await _doctorService.Delete(doctorId);

            // Assert

            _doctorRepositoryMock.Verify(repo => repo.Delete(doctorId), Times.Once);

            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);

        }

    }

}

