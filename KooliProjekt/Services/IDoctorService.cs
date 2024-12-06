using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IDoctorService
    {
        Task<PagedResult<Doctor>> List(int page, int pageSize, DoctorsSearch search = null);
        Task<Doctor> Get(int id);
        Task Save(Doctor list);
        Task Delete(int id);
    }
}
