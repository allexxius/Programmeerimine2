using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IDocumentService
    {
        Task<PagedResult<Document>> List(int page, int pageSize, Search.DocumentSearch search);
        Task<Document> Get(int id);
        Task Save(Document list);
        Task Delete(int id);
    }
}
