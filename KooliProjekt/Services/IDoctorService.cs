using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IDoctorService
    {
        // List meetod ilma otsinguparameetriteta
        Task<PagedResult<Doctor>> List(int page, int pageSize);

        // List meetod otsinguparameetritega
        Task<PagedResult<Doctor>> List(int page, int pageSize, DoctorSearch search);

        // Get meetod arsti ID järgi
        Task<Doctor> Get(int id);

        // Save meetod arsti salvestamiseks
        Task Save(Doctor doctor);

        // Delete meetod arsti kustutamiseks
        Task Delete(int id);
        Task Update(Doctor doctor);
        void Add(Doctor doctor);
    }
}