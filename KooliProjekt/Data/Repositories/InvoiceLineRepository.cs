using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Data;

namespace KooliProjekt.Data.Repositories
{
    public class InvoiceLineRepository : IInvoiceLineRepository
    {
        protected readonly ApplicationDbContext _context;

        public InvoiceLineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceLine> Get(int id)
        {
            return await _context.InvoiceLines.FindAsync(id);
        }

        public async Task<PagedResult<InvoiceLine>> List(int page, int pageSize)
        {
            return await _context.InvoiceLines
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
        }

        public async Task Save(InvoiceLine invoiceLine)
        {
            if (invoiceLine.Id == 0)
            {
                _context.InvoiceLines.Add(invoiceLine);
            }
            else
            {
                _context.InvoiceLines.Update(invoiceLine);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var invoiceLine = await _context.InvoiceLines.FindAsync(id);
            if (invoiceLine != null)
            {
                _context.InvoiceLines.Remove(invoiceLine);
                await _context.SaveChangesAsync();
            }
        }
    }
}