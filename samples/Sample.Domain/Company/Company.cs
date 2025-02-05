namespace Sample.Domain.Company
{
	using System.Collections.Generic;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;

	public class Company : Entity<Company, CompanyId>
	{
		public string Name { get; set; }

		public LegalType LegalType { get; set; }

		public Address Address { get; set; }

		[Reference("Company")]
		public IList<Company> Partners { get; set; }
	}
}
