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
    public class InvoiceLinesControllerTests
    {
        private readonly Mock<IInvoiceLineService> _invoiceLineServiceMock;
        private readonly InvoiceLinesController _controller;

        public InvoiceLinesControllerTests()
        {
            _invoiceLineServiceMock = new Mock<IInvoiceLineService>();
            _controller = new InvoiceLinesController(null); // _invoiceLineServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            int pageSize = 5;
            var data = new List<InvoiceLine>
            {
                new InvoiceLine { Id = 1, InvoiceId = 1001, Service = "Service A", Price = 50.00M },
                new InvoiceLine { Id = 2, InvoiceId = 1002, Service = "Service B", Price = 75.00M }
            };

            var pagedResult = new PagedResult<InvoiceLine> { Results = data };
            _invoiceLineServiceMock.Setup(x => x.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }

        [Fact]
        public async Task Details_should_return_correct_invoiceLine()
        {
            // Arrange
            int id = 1;
            var invoiceLine = new InvoiceLine { Id = id, InvoiceId = 1001, Service = "Service A", Price = 50.00M };
            _invoiceLineServiceMock.Setup(x => x.Get(id)).ReturnsAsync(invoiceLine);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoiceLine, result.Model);
        }

        [Fact]
        public async Task Create_should_save_invoiceLine_and_redirect_to_index()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 0, InvoiceId = 1001, Service = "Service A", Price = 50.00M };
            _invoiceLineServiceMock.Setup(x => x.Save(invoiceLine)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(invoiceLine) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_should_update_invoiceLine_and_redirect_to_index()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 1, InvoiceId = 1001, Service = "Service A", Price = 55.00M };
            _invoiceLineServiceMock.Setup(x => x.Save(invoiceLine)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(invoiceLine.Id, invoiceLine) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_should_remove_invoiceLine_and_redirect_to_index()
        {
            // Arrange
            int id = 1;
            _invoiceLineServiceMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
