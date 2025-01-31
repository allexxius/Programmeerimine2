using System.Threading.Tasks;

namespace KooliProjekt.Data.Repositories

{

    public interface IDocumentRepository : IBaseRepository<Document>

    {

    }

    public interface IBaseRepository<T> where T : class

    {

        Task<T> Get(int id);

        Task<PagedResult<T>> List(int page, int pageSize);

        Task Save(T item);

        Task Delete(int id);

    }

}
