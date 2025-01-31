using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public class TimeRepository : BaseRepository<Time>, ITimeRepository

    {

        public TimeRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Time> Get(int id)

        {

            return await DbContext.Set<Time>().FindAsync(id);

        }

        public override async Task<PagedResult<Time>> List(int page, int pageSize)

        {

            return await DbContext.Set<Time>()

                .OrderByDescending(x => x.Id)

                .GetPagedAsync(page, pageSize);

        }

        public override async Task Save(Time item)

        {

            if (item.Id == 0)

            {

                DbContext.Set<Time>().Add(item);

            }

            else

            {

                DbContext.Set<Time>().Update(item);

            }

            await DbContext.SaveChangesAsync();

        }

        public override async Task Delete(int id)

        {

            var time = await DbContext.Set<Time>().FindAsync(id);

            if (time != null)

            {

                DbContext.Set<Time>().Remove(time);

                await DbContext.SaveChangesAsync();

            }

        }

    }

}
