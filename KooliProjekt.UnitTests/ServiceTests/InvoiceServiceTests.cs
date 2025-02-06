using System.Threading.Tasks;
using Xunit;
using Moq;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Tests
{
    public class InvoiceServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly InvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _invoiceService = new InvoiceService(_mockContext.Object);
        }

        [Fact]
        public async Task Save_ShouldAddNewInvoice_WhenInvoiceIdIsZero()
        {
            // Arrange
            var invoice = new Invoice { Id = 0, Sum = 100, Date = DateTime.Now, Paid = false, VisitId = 1 };
            _mockContext.Setup(m => m.Invoices.Add(invoice)).Verifiable();

            // Act
            await _invoiceService.Save(invoice);

            // Assert
            _mockContext.Verify(m => m.Invoices.Add(invoice), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Save_ShouldUpdateExistingInvoice_WhenInvoiceIdIsNonZero()
        {
            // Arrange
            var invoice = new Invoice { Id = 1, Sum = 100, Date = DateTime.Now, Paid = false, VisitId = 1 };
            _mockContext.Setup(m => m.Invoices.Update(invoice)).Verifiable();

            // Act
            await _invoiceService.Save(invoice);

            // Assert
            _mockContext.Verify(m => m.Invoices.Update(invoice), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
