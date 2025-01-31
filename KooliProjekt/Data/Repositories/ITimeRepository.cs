namespace KooliProjekt.Data.Repositories
{
    public interface ITimeRepository
    {
        Task Delete(int id);
        Task<Time> Get(int id);
        Task<PagedResult<Time>> List(int page, int pageSize);
        Task Save(Time time);
    }
}