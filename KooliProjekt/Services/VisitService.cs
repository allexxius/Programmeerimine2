using KooliProjekt.Data;

using KooliProjekt.Data.Repositories;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Services

{

    public class VisitService : IVisitService

    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IVisitRepository _visitRepository;

        public VisitService(IUnitOfWork unitOfWork, IVisitRepository visitRepository)

        {

            _unitOfWork = unitOfWork;

            _visitRepository = visitRepository;

        }

        public async Task<PagedResult<Visit>> List(int page, int pageSize)

        {

            return await _visitRepository.List(page, pageSize);

        }

        public async Task<Visit> Get(int id)

        {

            return await _visitRepository.Get(id);

        }

        public async Task Save(Visit visit)

        {

            await _visitRepository.Save(visit);

            await _unitOfWork.CommitAsync();

        }

        public async Task Delete(int id)

        {

            await _visitRepository.Delete(id);

            await _unitOfWork.CommitAsync();

        }

    }

}
