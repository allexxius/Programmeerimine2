using KooliProjekt.Data;

using KooliProjekt.Models;

using KooliProjekt.Services;

using KooliProjekt.UnitTests.ServiceTests;

using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests

{

    public class InvoiceServiceTests : ServiceTestBase

    {

        private ApplicationDbContext _context;

        private InvoiceService _invoiceService;

        public InvoiceServiceTests()

        {

            _context = DbContext;

            _invoiceService = new InvoiceService(_context);

            // Add test data

            _context.Invoices.AddRange(new List<Invoice>

            {

                new Invoice { Id = 1, Date = DateTime.Now, Sum = 150.00m, Paid = false, VisitId = 10 },

                new Invoice { Id = 2, Date = DateTime.Now, Sum = 200.00m, Paid = true, VisitId = 20 },

                new Invoice { Id = 3, Date = DateTime.Now, Sum = 300.00m, Paid = false, VisitId = 30 }

            });

            _context.SaveChanges();

        }

        [Fact]

        public async Task List_WithoutFilters_ReturnsAllInvoices()

        {

            var page = 1;

            var pageSize = 10;

            var result = await _invoiceService.List(page, pageSize);

            Assert.Equal(3, result.Results.Count);

        }

        [Fact]

        public async Task Get_ExistingInvoiceId_ReturnsInvoice()

        {

            var invoiceId = 2;

            var result = await _invoiceService.Get(invoiceId);

            Assert.NotNull(result);

            Assert.Equal(200.00m, result.Sum);

        }

        [Fact]

        public async Task Get_NonExistingInvoiceId_ReturnsNull()

        {

            var invoiceId = 999;

            var result = await _invoiceService.Get(invoiceId);

            Assert.Null(result);

        }

        [Fact]

        public async Task Save_NewInvoice_AddsInvoiceToDatabase()

        {

            var newInvoice = new Invoice { Date = DateTime.Now, Sum = 400.00m, Paid = false, VisitId = 40 };

            await _invoiceService.Save(newInvoice);

            await _context.SaveChangesAsync();

            var result = await _context.Invoices.FindAsync(newInvoice.Id);

            Assert.NotNull(result);

            Assert.Equal(400.00m, result.Sum);

        }

        [Fact]

        public async Task Save_ExistingInvoice_UpdatesInvoiceInDatabase()

        {

            var existingInvoice = await _context.Invoices.FindAsync(1);

            existingInvoice.Sum = 175.00m;

            await _invoiceService.Save(existingInvoice);

            await _context.SaveChangesAsync();

            var result = await _context.Invoices.FindAsync(1);

            Assert.NotNull(result);

            Assert.Equal(175.00m, result.Sum);

        }

        [Fact]

        public async Task Delete_ExistingInvoiceId_RemovesInvoiceFromDatabase()

        {

            var invoiceId = 1;

            await _invoiceService.Delete(invoiceId);

            await _context.SaveChangesAsync();

            var result = await _context.Invoices.FindAsync(invoiceId);

            Assert.Null(result);

        }

        [Fact]

        public async Task Delete_NonExistingInvoiceId_DoesNothing()

        {

            var invoiceId = 999;

            await _invoiceService.Delete(invoiceId);

            await _context.SaveChangesAsync();

            var result = await _context.Invoices.FindAsync(invoiceId);

            Assert.Null(result);

        }

    }

}

