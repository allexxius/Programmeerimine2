using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Tests.Services
{
    public class VisitServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IVisitRepository> _visitRepositoryMock;
        private readonly VisitService _visitService;

        public VisitServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _visitRepositoryMock = new Mock<IVisitRepository>();
            _visitService = new VisitService(_unitOfWorkMock.Object, _visitRepositoryMock.Object);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var pagedResult = new PagedResult<Visit>();
            _visitRepositoryMock.Setup(repo => repo.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _visitService.List(page, pageSize);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_ShouldReturnVisit()
        {
            // Arrange
            var visitId = 1;
            var visit = new Visit { Id = visitId };
            _visitRepositoryMock.Setup(repo => repo.Get(visitId)).ReturnsAsync(visit);

            // Act
            var result = await _visitService.Get(visitId);

            // Assert
            Assert.Equal(visit, result);
        }

        [Fact]
        public async Task Save_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var visit = new Visit();
            _visitRepositoryMock.Setup(repo => repo.Save(visit)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _visitService.Save(visit);

            // Assert
            _visitRepositoryMock.Verify(repo => repo.Save(visit), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var visitId = 1;
            _visitRepositoryMock.Setup(repo => repo.Delete(visitId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _visitService.Delete(visitId);

            // Assert
            _visitRepositoryMock.Verify(repo => repo.Delete(visitId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}