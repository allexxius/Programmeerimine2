using KooliProjekt.Data;

using KooliProjekt.Data.Repositories;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Services

{

    public class TimeService : ITimeService

    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly ITimeRepository _timeRepository;

        public TimeService(IUnitOfWork unitOfWork, ITimeRepository timeRepository)

        {

            _unitOfWork = unitOfWork;

            _timeRepository = timeRepository;

        }

        public async Task<PagedResult<Time>> List(int page, int pageSize)

        {

            return await _timeRepository.List(page, pageSize);

        }

        public async Task<Time> Get(int id)

        {

            return await _timeRepository.Get(id);

        }

        public async Task Save(Time time)

        {

            await _timeRepository.Save(time);

            await _unitOfWork.CommitAsync();

        }

        public async Task Delete(int id)

        {

            await _timeRepository.Delete(id);

            await _unitOfWork.CommitAsync();

        }

    }

}
