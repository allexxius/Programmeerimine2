using KooliProjekt.Data;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services

{

    public class DoctorService : IDoctorService

    {

        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<PagedResult<Doctor>> List(int page, int pageSize)

        {

            return await _context.Doctors.GetPagedAsync(page, 5);

        }

        public async Task<Doctor> Get(int id)

        {

            return await _context.Doctors.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task Save(Doctor list)

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

            var Doctor = await _context.Doctors.FindAsync(id);

            if (Doctor != null)

            {

                _context.Doctors.Remove(Doctor);

                await _context.SaveChangesAsync();

            }

        }

    }

}
