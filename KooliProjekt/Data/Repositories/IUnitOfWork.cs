using System.Security.Policy;

namespace KooliProjekt.Data.Repositories

{

    public interface IUnitOfWork

    {

        Task BeginTransaction();

        Task Commit();

        Task Rollback();

        IDoctorRepository DoctorRepository { get; }

    }

}

