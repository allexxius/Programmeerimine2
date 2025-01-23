using KooliProjekt.Data;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services

{

    public class InvoiceService : IInvoiceService

    {

        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)

        {

            return await _context.Invoices.GetPagedAsync(page, 5);

        }

        public async Task<Invoice> Get(int id)

        {

            return await _context.Invoices.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task Save(Invoice list)

        {

            if (list.Id == 0)

            {

                _context.Add(list);

            }

            else

            {

                _context.Update(list);

            }

            await _context.SaveChangesAsync();

        }

        public async Task Delete(int id)

        {

            var Invoice = await _context.Invoices.FindAsync(id);

            if (Invoice != null)

            {

                _context.Invoices.Remove(Invoice);

                await _context.SaveChangesAsync();

            }

        }

    }

}

