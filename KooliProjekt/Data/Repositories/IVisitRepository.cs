namespace KooliProjekt.Data.Repositories
{
    public interface IVisitRepository
    {
        Task<Visit> Get(int id);
        Task<PagedResult<Visit>> List(int page, int pageSize);
    }
}