using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class

    {

        protected readonly ApplicationDbContext DbContext;

        protected BaseRepository(ApplicationDbContext context)

        {

            DbContext = context;

        }

        public virtual async Task<T> Get(int id)

        {

            return await DbContext.Set<T>().FindAsync(id);

        }

        public virtual async Task<PagedResult<T>> List(int page, int pageSize)

        {

            return await DbContext.Set<T>()
            .OrderByDescending(x => (x as Entity).Id)

                .GetPagedAsync(page, pageSize);

        }

        public virtual async Task Save(T item)

        {
            if ((item as Entity).Id == 0)

            {

                DbContext.Set<T>().Add(item);

            }

            else

            {

                DbContext.Set<T>().Update(item);

            }

            await DbContext.SaveChangesAsync();

        }

        public virtual async Task Delete(int id)

        {

            var entity = await DbContext.Set<T>().FindAsync(id);

            if (entity != null)

            {

                DbContext.Set<T>().Remove(entity);

                await DbContext.SaveChangesAsync();

            }

        }

    }

}
