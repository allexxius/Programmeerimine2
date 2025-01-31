using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Tests.Services
{
    public class DocumentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDocumentRepository> _documentRepositoryMock;
        private readonly DocumentService _documentService;

        public DocumentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _documentService = new DocumentService(_unitOfWorkMock.Object, _documentRepositoryMock.Object);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var pagedResult = new PagedResult<Document>();
            _documentRepositoryMock.Setup(repo => repo.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _documentService.List(page, pageSize);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_ShouldReturnDocument()
        {
            // Arrange
            var documentId = 1;
            var document = new Document { ID = documentId };
            _documentRepositoryMock.Setup(repo => repo.Get(documentId)).ReturnsAsync(document);

            // Act
            var result = await _documentService.Get(documentId);

            // Assert
            Assert.Equal(document, result);
        }

        [Fact]
        public async Task Save_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var document = new Document();
            _documentRepositoryMock.Setup(repo => repo.Save(document)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _documentService.Save(document);

            // Assert
            _documentRepositoryMock.Verify(repo => repo.Save(document), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var documentId = 1;
            _documentRepositoryMock.Setup(repo => repo.Delete(documentId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _documentService.Delete(documentId);

            // Assert
            _documentRepositoryMock.Verify(repo => repo.Delete(documentId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}