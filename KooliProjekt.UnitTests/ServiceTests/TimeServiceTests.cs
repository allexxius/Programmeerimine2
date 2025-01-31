using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Tests.Services
{
    public class TimeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITimeRepository> _timeRepositoryMock;
        private readonly TimeService _timeService;

        public TimeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _timeRepositoryMock = new Mock<ITimeRepository>();
            _timeService = new TimeService(_unitOfWorkMock.Object, _timeRepositoryMock.Object);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var pagedResult = new PagedResult<Time>();
            _timeRepositoryMock.Setup(repo => repo.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _timeService.List(page, pageSize);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_ShouldReturnTime()
        {
            // Arrange
            var timeId = 1;
            var time = new Time { Id = timeId };
            _timeRepositoryMock.Setup(repo => repo.Get(timeId)).ReturnsAsync(time);

            // Act
            var result = await _timeService.Get(timeId);

            // Assert
            Assert.Equal(time, result);
        }

        [Fact]
        public async Task Save_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var time = new Time();
            _timeRepositoryMock.Setup(repo => repo.Save(time)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _timeService.Save(time);

            // Assert
            _timeRepositoryMock.Verify(repo => repo.Save(time), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var timeId = 1;
            _timeRepositoryMock.Setup(repo => repo.Delete(timeId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _timeService.Delete(timeId);

            // Assert
            _timeRepositoryMock.Verify(repo => repo.Delete(timeId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}