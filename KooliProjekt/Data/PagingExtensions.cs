using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
<<<<<<< HEAD

{

    public static class PagingExtensions

    {

        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class

        {

            page = Math.Max(page, 1);

            if (pageSize == 0)

            {

                pageSize = 10;

            }

            var result = new PagedResult<T>

            {

                CurrentPage = page,

                PageSize = pageSize,

                RowCount = await query.CountAsync()

            };

            var pageCount = (double)result.RowCount / pageSize;

            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;

        }

    }

}

=======
{
    public static class PagingExtensions
    {

        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            page = Math.Max(page, 1);
            if (pageSize == 0)
            {
                pageSize = 10;
            }

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
>>>>>>> 3ab08cc95858c0f3d4ab2d2123111f2da03c6471
