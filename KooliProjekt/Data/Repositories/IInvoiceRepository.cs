namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceRepository
    {
        Task Delete(int id);
        Task<Invoice> Get(int id);
        Task<PagedResult<Invoice>> List(int page, int pageSize);
        Task Save(Invoice invoice);
    }
}