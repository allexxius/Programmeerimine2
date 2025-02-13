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
    public class InvoicesControllerTests
    {
        private readonly Mock<IInvoiceService> _invoiceServiceMock;
        private readonly InvoicesController _controller;

        public InvoicesControllerTests()
        {
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _controller = new InvoicesController(_invoiceServiceMock.Object);
        }

        // Index Action Tests
        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<Invoice>
            {
                new Invoice { Id = 1, Sum = 100 },
                new Invoice { Id = 2, Sum = 200 }
            };
            var pagedResult = new PagedResult<Invoice> { Results = data };

            _invoiceServiceMock
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
        public async Task Details_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithInvoice_WhenInvoiceExists()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoice);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoice, result.Model);
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
            var invoice = new Invoice { Id = 1, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Save(invoice))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(invoice) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceServiceMock.Verify(x => x.Save(invoice), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnViewWithInvoice_WhenModelStateIsInvalid()
        {
            // Arrange
            var invoice = new Invoice { Id = 1, Sum = 100 };
            _controller.ModelState.AddModelError("Sum", "Required");

            // Act
            var result = await _controller.Create(invoice) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoice, result.Model);
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
        public async Task Edit_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithInvoice_WhenInvoiceExists()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoice);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoice, result.Model);
        }

        // Edit (POST) Action Tests
        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenIdDoesNotMatch()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = 2, Sum = 100 };

            // Act
            var result = await _controller.Edit(id, invoice);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldCallSaveMethod_AndRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Save(invoice))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, invoice) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceServiceMock.Verify(x => x.Save(invoice), Times.Once);
        }

        [Fact]
        public async Task Edit_ShouldReturnViewWithInvoice_WhenModelStateIsInvalid()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };
            _controller.ModelState.AddModelError("Sum", "Required");

            // Act
            var result = await _controller.Edit(id, invoice) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task Edit_ShouldRethrowDbUpdateConcurrencyException_WhenInvoiceExists()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Save(invoice))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoice);

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, invoice));
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenInvoiceDoesNotExist_AndConcurrencyExceptionOccurs()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Save(invoice))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Edit(id, invoice);

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
        public async Task Delete_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnViewWithInvoice_WhenInvoiceExists()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoice);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal(invoice, result.Model);
        }

        // DeleteConfirmed (POST) Action Tests
        [Fact]
        public async Task DeleteConfirmed_ShouldCallDeleteMethod_AndRedirectToIndex()
        {
            // Arrange
            int id = 1;

            _invoiceServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        // InvoiceExists Method Tests
        [Fact]
        public void InvoiceExists_ShouldReturnTrue_WhenInvoiceExists()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(invoice);

            // Act
            var result = _controller.InvoiceExists(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void InvoiceExists_ShouldReturnFalse_WhenInvoiceDoesNotExist()
        {
            // Arrange
            int id = 1;

            _invoiceServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync((Invoice)null);

            // Act
            var result = _controller.InvoiceExists(id);

            // Assert
            Assert.False(result);
        }
    }
}
