using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public class VisitRepository : BaseRepository<Visit>, IVisitRepository

    {

        public VisitRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Visit> Get(int id)

        {

            return await DbContext.Set<Visit>().FindAsync(id);

        }

        public override async Task<PagedResult<Visit>> List(int page, int pageSize)

        {

            return await DbContext.Set<Visit>()

                .OrderByDescending(x => x.Id)

                .GetPagedAsync(page, pageSize);

        }

        public override async Task Save(Visit item)

        {

            if (item.Id == 0)

            {

                DbContext.Set<Visit>().Add(item);

            }

            else

            {

                DbContext.Set<Visit>().Update(item);

            }

            await DbContext.SaveChangesAsync();

        }

        public override async Task Delete(int id)

        {

            var visit = await DbContext.Set<Visit>().FindAsync(id);

            if (visit != null)

            {

                DbContext.Set<Visit>().Remove(visit);

                await DbContext.SaveChangesAsync();

            }

        }

    }

}
