namespace Sample.Domain.Company
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;

	public class Company : AggregateRoot<Company, CompanyId>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public LegalType LegalType { get; set; }

		[Reference("Company")]
		public IList<Company> Partners { get; set; }
	}
}
