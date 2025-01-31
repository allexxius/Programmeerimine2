using KooliProjekt.Data;

using KooliProjekt.Data.Repositories;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Services

{

    public class DoctorService : IDoctorService

    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IUnitOfWork unitOfWork, IDoctorRepository doctorRepository)

        {

            _unitOfWork = unitOfWork;

            _doctorRepository = doctorRepository;

        }

        public async Task<Doctor> Get(int id)

        {

            return await _doctorRepository.Get(id);

        }

        public async Task Save(Doctor doctor)

        {

            await _doctorRepository.Save(doctor);

            await _unitOfWork.CommitAsync();

        }

        public async Task Delete(int id)

        {

            await _doctorRepository.Delete(id);

            await _unitOfWork.CommitAsync();

        }

        public async Task<PagedResult<Doctor>> List(int page, int pageSize, DoctorsSearch search)
        {
            return await _doctorRepository.List(page, pageSize);
        }
    }

}
