using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository

    {

        public DocumentRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Document> Get(int id)

        {

            return await DbContext.Set<Document>().FindAsync(id);

        }

        public override async Task<PagedResult<Document>> List(int page, int pageSize)

        {

            return await DbContext.Set<Document>()

                .OrderByDescending(x => x.Id)

                .GetPagedAsync(page, pageSize);

        }

        public override async Task Save(Document item)

        {

            if (item.Id == 0)

            {

                DbContext.Set<Document>().Add(item);

            }

            else

            {

                DbContext.Set<Document>().Update(item);

            }

            await DbContext.SaveChangesAsync();

        }

        public override async Task Delete(int id)

        {

            var document = await DbContext.Set<Document>().FindAsync(id);

            if (document != null)

            {

                DbContext.Set<Document>().Remove(document);

                await DbContext.SaveChangesAsync();

            }

        }

    }

}
