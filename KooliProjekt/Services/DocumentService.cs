using KooliProjekt.Data;

using KooliProjekt.Data.Repositories;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Services

{

    public class DocumentService : IDocumentService

    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IUnitOfWork unitOfWork, IDocumentRepository documentRepository)

        {

            _unitOfWork = unitOfWork;

            _documentRepository = documentRepository;

        }

        public async Task<PagedResult<Document>> List(int page, int pageSize)

        {

            return await _documentRepository.List(page, pageSize);

        }

        public async Task<Document> Get(int id)

        {

            return await _documentRepository.Get(id);

        }

        public async Task Save(Document document)

        {

            await _documentRepository.Save(document);

            await _unitOfWork.CommitAsync();

        }

        public async Task Delete(int id)

        {

            await _documentRepository.Delete(id);

            await _unitOfWork.CommitAsync();

        }

    }

}
