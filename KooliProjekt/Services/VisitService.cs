using KooliProjekt.Data;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services

{

    public class VisitService : IVisitService

    {

        private readonly ApplicationDbContext _context;

        public VisitService(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<PagedResult<Visit>> List(int page, int pageSize)

        {

            return await _context.Visits.GetPagedAsync(page, 5);

        }

        public async Task<Visit> Get(int id)

        {

            return await _context.Visits.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task Save(Visit list)

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

            var Visit = await _context.Visits.FindAsync(id);

            if (Visit != null)

            {

                _context.Visits.Remove(Visit);

                await _context.SaveChangesAsync();

            }

        }

    }

}

