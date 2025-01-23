using KooliProjekt.Data;

namespace KooliProjekt.Services

{

    public interface IVisitService

    {

        Task<PagedResult<Visit>> List(int page, int pageSize);

        Task<Visit> Get(int id);

        Task Save(Visit list);

        Task Delete(int id);

    }

}

