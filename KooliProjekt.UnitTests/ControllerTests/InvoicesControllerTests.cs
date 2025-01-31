using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Invoice>
            {
                new Invoice { Id = 1, Sum = 1 },
                new Invoice { Id = 2, Sum = 1 }
            };
            var pagedResult = new PagedResult<Invoice> { Results = data };
            _invoiceServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }

        [Fact]
        public async Task Details_should_return_not_found_for_invalid_id()
        {
            // Arrange
            int id = 999;
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_for_valid_id()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public void Create_GET_should_return_view()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_POST_should_redirect_to_index_when_model_is_valid()
        {
            // Arrange
            var invoice = new Invoice { Id = 1, Sum = 100 };
            _invoiceServiceMock.Setup(x => x.Save(invoice)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(invoice) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_POST_should_return_view_with_model_when_model_is_invalid()
        {
            // Arrange
            var invoice = new Invoice { Id = 1, Sum = 100 };
            _controller.ModelState.AddModelError("Sum", "Required");

            // Act
            var result = await _controller.Create(invoice) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task Edit_GET_should_return_not_found_for_invalid_id()
        {
            // Arrange
            int id = 999;
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_GET_should_return_view_with_model_for_valid_id()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task Edit_POST_should_redirect_to_index_when_model_is_valid()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };
            _invoiceServiceMock.Setup(x => x.Save(invoice)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(id, invoice) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_POST_should_return_view_with_model_when_model_is_invalid()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };
            _controller.ModelState.AddModelError("Sum", "Required");

            // Act
            var result = await _controller.Edit(id, invoice) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task Delete_GET_should_return_not_found_for_invalid_id()
        {
            // Arrange
            int id = 999;
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_GET_should_return_view_with_model_for_valid_id()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, Sum = 100 };
            _invoiceServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoice);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            // Arrange
            int id = 1;
            _invoiceServiceMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}