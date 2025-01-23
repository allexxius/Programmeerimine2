namespace KooliProjekt.Data.Repositories
{
    public interface IDocumentRepository
    {
        Task<Document> Get(int id);
        Task<PagedResult<Document>> List(int page, int pageSize);
    }
}