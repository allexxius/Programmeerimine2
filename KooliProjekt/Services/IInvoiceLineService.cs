using KooliProjekt.Data;

namespace KooliProjekt.Services

{

    public interface IInvoiceLineService

    {

        Task<PagedResult<InvoiceLine>> List(int page, int pageSize);

        Task<InvoiceLine> Get(int id);

        Task Save(InvoiceLine list);

        Task Delete(int id);

    }

}

