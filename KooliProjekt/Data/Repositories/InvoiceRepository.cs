using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository

    {

        public InvoiceRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Invoice> Get(int id)

        {

            return await DbContext.Set<Invoice>().FindAsync(id);

        }

        public override async Task<PagedResult<Invoice>> List(int page, int pageSize)

        {

            return await DbContext.Set<Invoice>()

                .OrderByDescending(x => x.Id)

                .GetPagedAsync(page, pageSize);

        }

        public override async Task Save(Invoice item)

        {

            if (item.Id == 0)

            {

                DbContext.Set<Invoice>().Add(item);

            }

            else

            {

                DbContext.Set<Invoice>().Update(item);

            }

            await DbContext.SaveChangesAsync();

        }

        public override async Task Delete(int id)

        {

            var invoice = await DbContext.Set<Invoice>().FindAsync(id);

            if (invoice != null)

            {

                DbContext.Set<Invoice>().Remove(invoice);

                await DbContext.SaveChangesAsync();

            }

        }

    }

}
