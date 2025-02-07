using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class InvoiceLineServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly InvoiceLineService _invoiceLineService;

        public InvoiceLineServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Loob igale testile uue andmebaasi
                .Options;

            _context = new ApplicationDbContext(options);
            _invoiceLineService = new InvoiceLineService(_context);
        }

        public void Dispose()
        {
            _context.Dispose(); // Vabasta ressursid pärast testi
        }

        [Fact]
        public async Task List_ReturnsPagedResult()
        {
            // Arrange
            _context.InvoiceLines.AddRange(
                new InvoiceLine { Id = 1, InvoiceId = 101, Service = "Consulting", Price = 100 },
                new InvoiceLine { Id = 2, InvoiceId = 102, Service = "Support", Price = 200 }
            );
            await _context.SaveChangesAsync(); // Salvesta testandmed

            // Act
            var result = await _invoiceLineService.List(1, 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count());
        }

        [Fact]
        public async Task Get_ReturnsInvoiceLine()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 1, InvoiceId = 101, Service = "Consulting", Price = 100 };
            _context.InvoiceLines.Add(invoiceLine);
            await _context.SaveChangesAsync();

            // Act
            var result = await _invoiceLineService.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Save_AddsNewInvoiceLine()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 0, InvoiceId = 103, Service = "Development", Price = 300 };

            // Act
            await _invoiceLineService.Save(invoiceLine);

            // Assert
            var savedInvoiceLine = await _context.InvoiceLines.FirstOrDefaultAsync(i => i.InvoiceId == 103);
            Assert.NotNull(savedInvoiceLine);
            Assert.Equal("Development", savedInvoiceLine.Service);
        }

        [Fact]
        public async Task Save_UpdatesExistingInvoiceLine()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 1, InvoiceId = 101, Service = "Consulting", Price = 100 };
            _context.InvoiceLines.Add(invoiceLine);
            await _context.SaveChangesAsync();

            // Act
            invoiceLine.Service = "Updated Service";
            await _invoiceLineService.Save(invoiceLine);

            // Assert
            var updatedInvoiceLine = await _context.InvoiceLines.FindAsync(1);
            Assert.NotNull(updatedInvoiceLine);
            Assert.Equal("Updated Service", updatedInvoiceLine.Service);
        }

        [Fact]
        public async Task Delete_RemovesInvoiceLine()
        {
            // Arrange
            var invoiceLine = new InvoiceLine { Id = 1, InvoiceId = 101, Service = "Consulting", Price = 100 };
            _context.InvoiceLines.Add(invoiceLine);
            await _context.SaveChangesAsync();

            // Act
            await _invoiceLineService.Delete(1);

            // Assert
            var deletedInvoiceLine = await _context.InvoiceLines.FindAsync(1);
            Assert.Null(deletedInvoiceLine);
        }

        [Fact]
        public async Task Delete_DoesNothingWhenInvoiceLineNotFound()
        {
            // Act
            await _invoiceLineService.Delete(999); // Kustuta olematu kirje

            // Assert
            var count = await _context.InvoiceLines.CountAsync();
            Assert.Equal(0, count);
        }
    }
}
