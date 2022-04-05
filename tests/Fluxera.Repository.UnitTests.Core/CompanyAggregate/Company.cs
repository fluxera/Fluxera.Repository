namespace Fluxera.Repository.UnitTests.Core.CompanyAggregate
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Company : AggregateRoot<Company, Guid>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public LegalType LegalType { get; set; }
	}
}
