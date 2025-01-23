namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> Get(int id);
        Task<PagedResult<Invoice>> List(int page, int pageSize);
    }
}