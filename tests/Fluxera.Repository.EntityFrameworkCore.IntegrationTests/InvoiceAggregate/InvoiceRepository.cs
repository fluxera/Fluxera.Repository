namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate
{
	public class InvoiceRepository : Repository<Invoice, string>, IInvoiceRepository
	{
		/// <inheritdoc />
		public InvoiceRepository(IRepository<Invoice, string> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
