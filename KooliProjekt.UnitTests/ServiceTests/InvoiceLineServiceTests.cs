using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Tests.Services
{
    public class InvoiceLineServiceTests
    {
        private readonly Mock<IInvoiceLineRepository> _invoiceLineRepositoryMock;
        private readonly InvoiceLineService _invoiceLineService;

        public InvoiceLineServiceTests()
        {
            _invoiceLineRepositoryMock = new Mock<IInvoiceLineRepository>();
            _invoiceLineService = new InvoiceLineService(_invoiceLineRepositoryMock.Object);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var pagedResult = new PagedResult<InvoiceLine>();
            _invoiceLineRepositoryMock.Setup(repo => repo.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _invoiceLineService.List(page, pageSize);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_ShouldReturnInvoiceLine()
        {
            // Arrange
            var invoiceLineId = 1;
            var invoiceLine = new InvoiceLine { Id = invoiceLineId };
            _invoiceLineRepositoryMock.Setup(repo => repo.Get(invoiceLineId)).ReturnsAsync(invoiceLine);

            // Act
            var result = await _invoiceLineService.Get(invoiceLineId);

            // Assert
            Assert.Equal(invoiceLine, result);
        }

        [Fact]
        public async Task Save_ShouldCallRepository()
        {
            // Arrange
            var invoiceLine = new InvoiceLine();
            _invoiceLineRepositoryMock.Setup(repo => repo.Save(invoiceLine)).Returns(Task.CompletedTask);

            // Act
            await _invoiceLineService.Save(invoiceLine);

            // Assert
            _invoiceLineRepositoryMock.Verify(repo => repo.Save(invoiceLine), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepository()
        {
            // Arrange
            var invoiceLineId = 1;
            _invoiceLineRepositoryMock.Setup(repo => repo.Delete(invoiceLineId)).Returns(Task.CompletedTask);

            // Act
            await _invoiceLineService.Delete(invoiceLineId);

            // Assert
            _invoiceLineRepositoryMock.Verify(repo => repo.Delete(invoiceLineId), Times.Once);
        }
    }
}