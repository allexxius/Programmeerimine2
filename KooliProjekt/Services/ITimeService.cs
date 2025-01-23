using KooliProjekt.Data;

namespace KooliProjekt.Services

{

    public interface ITimeService

    {

        Task<PagedResult<Time>> List(int page, int pageSize);

        Task<Time> Get(int id);

        Task Save(Time list);

        Task Delete(int id);

    }

}

