using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository

    {

        public DoctorRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Doctor> Get(int id)

        {

            return await DbContext.Set<Doctor>().FindAsync(id);

        }

        public override async Task<PagedResult<Doctor>> List(int page, int pageSize)

        {

            return await DbContext.Set<Doctor>()

                .OrderByDescending(x => x.Id)

                .GetPagedAsync(page, pageSize);

        }

        public override async Task Save(Doctor item)

        {

            if (item.Id == 0)

            {

                DbContext.Set<Doctor>().Add(item);

            }

            else

            {

                DbContext.Set<Doctor>().Update(item);

            }

            await DbContext.SaveChangesAsync();

        }

        public override async Task Delete(int id)

        {

            var doctor = await DbContext.Set<Doctor>().FindAsync(id);

            if (doctor != null)

            {

                DbContext.Set<Doctor>().Remove(doctor);

                await DbContext.SaveChangesAsync();

            }

        }

    }

}
