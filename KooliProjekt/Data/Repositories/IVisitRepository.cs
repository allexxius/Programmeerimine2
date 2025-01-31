namespace KooliProjekt.Data.Repositories
{
    public interface IVisitRepository
    {
        Task Delete(int id);
        Task<Visit> Get(int id);
        Task<PagedResult<Visit>> List(int page, int pageSize);
        Task Save(Visit visit);
    }
}