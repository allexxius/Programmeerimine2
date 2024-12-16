using KooliProjekt.Data;

namespace KooliProjekt.Services
<<<<<<< HEAD

{

    public interface IDoctorService

    {

        Task<PagedResult<Doctor>> List(int page, int pageSize);

        Task<Doctor> Get(int id);

        Task Save(Doctor list);

        Task Delete(int id);

    }

}

=======
{
    public interface IDoctorService
    {
        Task<PagedResult<Doctor>> List(int page, int pageSize);
        Task<Doctor> Get(int id);
        Task Save(Doctor list);
        Task Delete(int id);
    }
}
>>>>>>> 3ab08cc95858c0f3d4ab2d2123111f2da03c6471
