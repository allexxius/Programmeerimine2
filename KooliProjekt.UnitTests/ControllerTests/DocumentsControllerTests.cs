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
    public class DocumentsControllerTests
    {
        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly DocumentsController _controller;

        public DocumentsControllerTests()
        {
            _documentServiceMock = new Mock<IDocumentService>();
            _controller = new DocumentsController(_documentServiceMock.Object);
        }

        // Index Action Tests
        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<Document>
            {
                new Document { Id = 1, Type = "Test 1" },
                new Document { Id = 2, Type = "Test 2" }
            };
            var pagedResult = new PagedResult<Document> { Results = data };

            _documentServiceMock
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
        public async Task Details_ShouldReturnNotFound_WhenDocumentDoesNotExist()
        {
            // Arrange
            int id = 1;

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Document)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithDocument_WhenDocumentExists()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(document);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(document, result.Model);
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
            var document = new Document { Id = 1, Type = "Test Document" };
            _controller.ModelState.AddModelError("Type", "Required");

            // Act
            var result = await _controller.Create(document) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(document, result.Model);
        }

        [Fact]
        public async Task Create_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var document = new Document { Id = 1, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Save(document))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(document) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _documentServiceMock.Verify(x => x.Save(document), Times.Once);
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
        public async Task Edit_ShouldReturnNotFound_WhenDocumentDoesNotExist()
        {
            // Arrange
            int id = 1;

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Document)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithDocument_WhenDocumentExists()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(document);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(document, result.Model);
        }

        // Edit (POST) Action Tests
        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdDoesNotMatch()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = 2, Type = "Test Document" };

            // Act
            var result = await _controller.Edit(id, document);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithDocument_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };
            _controller.ModelState.AddModelError("Type", "Required");

            // Act
            var result = await _controller.Edit(id, document) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(document, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Save(document))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, document) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _documentServiceMock.Verify(x => x.Save(document), Times.Once);
        }

        [Fact]
        public async Task Edit_ShouldRethrowDbUpdateConcurrencyException_WhenDocumentExists()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Save(document))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(document);

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, document));
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenDocumentDoesNotExist_AndConcurrencyExceptionOccurs()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Save(document))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Document)null);

            // Act
            var result = await _controller.Edit(id, document);

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
        public async Task Delete_ShouldReturnNotFound_WhenDocumentDoesNotExist()
        {
            // Arrange
            int id = 1;

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Document)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithDocument_WhenDocumentExists()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id = id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(document);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(document, result.Model);
        }

        // DeleteConfirmed (POST) Action Tests
        [Fact]
        public async Task DeleteConfirmed_ShouldCallDeleteMethod_AndRedirectToIndex()
        {
            // Arrange
            int id = 1;

            _documentServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _documentServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        // DocumentExists Method Tests
        [Fact]
        public void DocumentExists_ShouldReturnTrue_WhenDocumentExists()
        {
            // Arrange
            int id = 1;
            var document = new Document { Id= id, Type = "Test Document" };

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(document);

            // Act
            var result = _controller.DocumentExists(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DocumentExists_ShouldReturnFalse_WhenDocumentDoesNotExist()
        {
            // Arrange
            int id = 1;

            _documentServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Document)null);

            // Act
            var result = _controller.DocumentExists(id);

            // Assert
            Assert.False(result);
        }
    }
}
