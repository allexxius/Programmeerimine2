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
    public class InvoiceLinesControllerTests
    {
        private readonly Mock<IInvoiceLineService> _invoiceLineServiceMock;
        private readonly InvoiceLinesController _controller;

        public InvoiceLinesControllerTests()
        {
            _invoiceLineServiceMock = new Mock<IInvoiceLineService>();
            _controller = new InvoiceLinesController(_invoiceLineServiceMock.Object);
        }

        // Index Action Tests
        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<InvoiceLine>
            {
                new InvoiceLine { Id = 1, Service = "Test 1" },
                new InvoiceLine { Id = 2, Service = "Test 2" }
            };
            var pagedResult = new PagedResult<InvoiceLine> { Results = data };

            _invoiceLineServiceMock
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
        public async Task Details_ShouldReturnNotFound_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((InvoiceLine)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithInvoiceLine_WhenInvoiceLineExists()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoiceLine);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoiceLine, result.Model);
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
        public async Task Create_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 1, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Save(invoiceLine))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(invoiceLine) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceLineServiceMock.Verify(x => x.Save(invoiceLine), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 1, Service = "Test Service" };
            _controller.ModelState.AddModelError("Service", "Required");

            // Act
            var result = await _controller.Create(invoiceLine) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoiceLine, result.Model);
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
        public async Task Edit_ShouldReturnNotFound_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((InvoiceLine)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithInvoiceLine_WhenInvoiceLineExists()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoiceLine);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoiceLine, result.Model);
        }

        // Edit (POST) Action Tests
        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdDoesNotMatch()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = 2, Service = "Test Service" };

            // Act
            var result = await _controller.Edit(id, invoiceLine);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithInvoiceLine_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };
            _controller.ModelState.AddModelError("Service", "Required");

            // Act
            var result = await _controller.Edit(id, invoiceLine) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoiceLine, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Save(invoiceLine))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, invoiceLine) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceLineServiceMock.Verify(x => x.Save(invoiceLine), Times.Once);
        }

        [Fact]
        public async Task Edit_ShouldRethrowDbUpdateConcurrencyException_WhenInvoiceLineExists()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Save(invoiceLine))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoiceLine);

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, invoiceLine));
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenInvoiceLineDoesNotExist_AndConcurrencyExceptionOccurs()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Save(invoiceLine))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((InvoiceLine)null);

            // Act
            var result = await _controller.Edit(id, invoiceLine);

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
        public async Task Delete_ShouldReturnNotFound_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((InvoiceLine)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithInvoiceLine_WhenInvoiceLineExists()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoiceLine);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoiceLine, result.Model);
        }

        // DeleteConfirmed (POST) Action Tests
        [Fact]
        public async Task DeleteConfirmed_ShouldCallDeleteMethod_AndRedirectToIndex()
        {
            // Arrange
            int id = 1;

            _invoiceLineServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceLineServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        // DocumentExists Method Tests
        [Fact]
        public void DocumentExists_ShouldReturnTrue_WhenInvoiceLineExists()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, Service = "Test Service" };

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoiceLine);

            // Act
            var result = _controller.DocumentExists(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DocumentExists_ShouldReturnFalse_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceLineServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((InvoiceLine)null);

            // Act
            var result = _controller.DocumentExists(id);

            // Assert
            Assert.False(result);
        }
    }
}
