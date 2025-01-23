using KooliProjekt.Data;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services

{

    public class InvoiceLineService : IInvoiceLineService

    {

        private readonly ApplicationDbContext _context;

        public InvoiceLineService(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<PagedResult<InvoiceLine>> List(int page, int pageSize)

        {

            return await _context.InvoiceLines.GetPagedAsync(page, 5);

        }

        public async Task<InvoiceLine> Get(int id)

        {

            return await _context.InvoiceLines.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task Save(InvoiceLine list)

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

            var InvoiceLine = await _context.Doctors.FindAsync(id);

            if (InvoiceLine != null)

            {

                _context.Doctors.Remove(InvoiceLine);

                await _context.SaveChangesAsync();

            }

        }

    }

}

