using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services

{

    public class DocumentService : IDocumentService

    {

        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<PagedResult<Document>> List(int page, int pageSize)

        {

            return await _context.Documents.GetPagedAsync(page, 5);

        }

        public async Task<Document> Get(int id)

        {

            return await _context.Documents.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task Save(Document list)

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

            var Document = await _context.Documents.FindAsync(id);

            if (Document != null)

            {

                _context.Documents.Remove(Document);

                await _context.SaveChangesAsync();

            }

        }

        public async Task<PagedResult<Document>> List(int page, int pageSize, DocumentSearch search)
        {
            var query = _context.Documents.AsQueryable();

            if(search != null && !string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(doc => doc.File.Contains(search.Keyword));
            }

            var result = await query.GetPagedAsync(page, pageSize);
            return result;
        }
    }

}