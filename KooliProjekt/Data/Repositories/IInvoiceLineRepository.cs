namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceLineRepository
    {
        Task Delete(int id);
        Task<InvoiceLine> Get(int id);
        Task<PagedResult<InvoiceLine>> List(int page, int pageSize);
        Task Save(InvoiceLine invoiceLine);
    }
}