using KooliProjekt.Data;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services

{

    public class TimeService : ITimeService

    {

        private readonly ApplicationDbContext _context;

        public TimeService(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<PagedResult<Time>> List(int page, int pageSize)

        {

            return await _context.Times.GetPagedAsync(page, 5);

        }

        public async Task<Time> Get(int id)

        {

            return await _context.Times.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task Save(Time list)

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

            var Time = await _context.Times.FindAsync(id);

            if (Time != null)

            {

                _context.Times.Remove(Time);

                await _context.SaveChangesAsync();

            }

        }

    }

}

