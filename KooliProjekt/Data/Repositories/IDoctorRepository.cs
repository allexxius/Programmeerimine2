namespace KooliProjekt.Data.Repositories
{
    public interface IDoctorRepository
    {
        Task Delete(int id);
        Task<Doctor> Get(int id);
        Task<PagedResult<Doctor>> List(int page, int pageSize);
        Task Save(Doctor doctor);
    }
}