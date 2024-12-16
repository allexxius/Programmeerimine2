namespace KooliProjekt.Data.Repositories

{

    public interface IDoctorRepository

    {

        Task<Doctor> Get(int id);

        Task<PagedResult<Doctor>> List(int page, int pageSize);

    }

}
