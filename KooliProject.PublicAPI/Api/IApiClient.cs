using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.PublicAPI
{
    public interface IApiClient
    {
        Task<Result<Doctor>> Get(int id);
        Task<Result<List<Doctor>>> List();
        Task<Result> Save(Doctor list);
        Task<Result> Delete(int id);
    }
}