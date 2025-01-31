using KooliProjekt.Data;

using KooliProjekt.Data.Repositories;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace KooliProjekt.Services

{

    public class InvoiceService : IInvoiceService

    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IUnitOfWork unitOfWork, IInvoiceRepository invoiceRepository)

        {

            _unitOfWork = unitOfWork;

            _invoiceRepository = invoiceRepository;

        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)

        {

            return await _invoiceRepository.List(page, pageSize);

        }

        public async Task<Invoice> Get(int id)

        {

            return await _invoiceRepository.Get(id);

        }

        public async Task Save(Invoice invoice)

        {

            await _invoiceRepository.Save(invoice);

            await _unitOfWork.CommitAsync();

        }

        public async Task Delete(int id)

        {

            await _invoiceRepository.Delete(id);

            await _unitOfWork.CommitAsync();

        }

    }

}
