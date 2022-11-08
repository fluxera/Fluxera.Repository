namespace Sample.Domain.Company
{
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Company : AggregateRoot<Company, CompanyId>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public LegalType LegalType { get; set; }
	}
}
