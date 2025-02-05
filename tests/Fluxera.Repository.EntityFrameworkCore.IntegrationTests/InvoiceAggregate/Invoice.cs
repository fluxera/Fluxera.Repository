// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate
{
	using System.Collections.Generic;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;

	public class Invoice : Entity<Invoice, string>
	{
		public string CustomerName { get; set; }

		[Reference<InvoiceItem>]
		public IList<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
	}
}
