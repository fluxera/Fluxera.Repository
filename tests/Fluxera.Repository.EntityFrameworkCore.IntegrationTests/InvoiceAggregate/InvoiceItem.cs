namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate
{
	using Fluxera.Entity;

	public class InvoiceItem : Entity<InvoiceItem, string>
	{
		public string Name { get; set; }

		public decimal Amount { get; set; }

		public Invoice Invoice { get; set; }
	}
}
