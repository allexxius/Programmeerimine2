namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceLineRepository
    {
        Task<InvoiceLine> Get(int id);
        Task<PagedResult<InvoiceLine>> List(int page, int pageSize);
    }
}