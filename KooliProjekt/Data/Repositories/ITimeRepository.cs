namespace KooliProjekt.Data.Repositories
{
    public interface ITimeRepository
    {
        Task<Time> Get(int id);
        Task<PagedResult<Time>> List(int page, int pageSize);
    }
}