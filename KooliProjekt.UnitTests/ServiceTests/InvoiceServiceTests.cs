using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Tests.Services
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly InvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _invoiceService = new InvoiceService(_unitOfWorkMock.Object, _invoiceRepositoryMock.Object);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var pagedResult = new PagedResult<Invoice>();
            _invoiceRepositoryMock.Setup(repo => repo.List(page, pageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _invoiceService.List(page, pageSize);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_ShouldReturnInvoice()
        {
            // Arrange
            var invoiceId = 1;
            var invoice = new Invoice { Id = invoiceId };
            _invoiceRepositoryMock.Setup(repo => repo.Get(invoiceId)).ReturnsAsync(invoice);

            // Act
            var result = await _invoiceService.Get(invoiceId);

            // Assert
            Assert.Equal(invoice, result);
        }

        [Fact]
        public async Task Save_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var invoice = new Invoice();
            _invoiceRepositoryMock.Setup(repo => repo.Save(invoice)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _invoiceService.Save(invoice);

            // Assert
            _invoiceRepositoryMock.Verify(repo => repo.Save(invoice), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryAndCommit()
        {
            // Arrange
            var invoiceId = 1;
            _invoiceRepositoryMock.Setup(repo => repo.Delete(invoiceId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            await _invoiceService.Delete(invoiceId);

            // Assert
            _invoiceRepositoryMock.Verify(repo => repo.Delete(invoiceId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}