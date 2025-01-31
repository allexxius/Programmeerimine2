using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class InvoiceLineService : IInvoiceLineService
    {
        private readonly IInvoiceLineRepository _invoiceLineRepository;

        public InvoiceLineService(IInvoiceLineRepository invoiceLineRepository)
        {
            _invoiceLineRepository = invoiceLineRepository;
        }

        public async Task<PagedResult<InvoiceLine>> List(int page, int pageSize)
        {
            return await _invoiceLineRepository.List(page, pageSize);
        }

        public async Task<InvoiceLine> Get(int id)
        {
            return await _invoiceLineRepository.Get(id);
        }

        public async Task Save(InvoiceLine invoiceLine)
        {
            await _invoiceLineRepository.Save(invoiceLine);
        }

        public async Task Delete(int id)
        {
            await _invoiceLineRepository.Delete(id);
        }
    }
}