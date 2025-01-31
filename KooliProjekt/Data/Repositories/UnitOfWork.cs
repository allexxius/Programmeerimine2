
namespace KooliProjekt.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context,
            IDoctorRepository doctorRepository,
            IDocumentRepository documentRepository,
            IInvoiceRepository invoiceRepository,
            IInvoiceLineRepository invoiceLineRepository,
            ITimeRepository timeRepository,
            IVisitRepository visitRepository)
        {
            _context = context;

            DoctorRepository = doctorRepository;
            DocumentRepository = documentRepository;
            InvoiceRepository = invoiceRepository;
            InvoiceLineRepository = invoiceLineRepository;
            TimeRepository = timeRepository;
            VisitRepository = visitRepository;
        }

        public IDoctorRepository DoctorRepository { get; private set; }
        public IDocumentRepository DocumentRepository { get; private set; }
        public IInvoiceRepository InvoiceRepository { get; private set; }
        public IInvoiceLineRepository InvoiceLineRepository { get; private set; }
        public ITimeRepository TimeRepository { get; private set; }
        public IVisitRepository VisitRepository { get; private set; }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
