using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List meetod ilma otsinguparameetriteta
        public async Task<PagedResult<Doctor>> List(int page, int pageSize)
        {
            return await _context.Doctors.GetPagedAsync(page, pageSize);
        }

        // List meetod otsinguparameetritega
        public async Task<PagedResult<Doctor>> List(int page, int pageSize, DoctorSearch search)
        {
            var query = _context.Doctors.AsQueryable();

            // Rakenda otsinguparameetrid
            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(d => d.Name.Contains(search.Keyword));
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        // Get meetod arsti ID järgi
        public async Task<Doctor> Get(int id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(m => m.Id == id);
        }

        // Save meetod arsti salvestamiseks
        public async Task Save(Doctor doctor)
        {
            if (doctor.Id == 0)
            {
                _context.Add(doctor);
            }
            else
            {
                _context.Update(doctor);
            }

            await _context.SaveChangesAsync();
        }

        // Delete meetod arsti kustutamiseks
        public async Task Delete(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }
    }
}