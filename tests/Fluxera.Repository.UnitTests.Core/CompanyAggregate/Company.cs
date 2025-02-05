namespace Fluxera.Repository.UnitTests.Core.CompanyAggregate
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Company : Entity<Company, string>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public LegalType LegalType { get; set; }

		public Guid Guid { get; set; } = Guid.NewGuid();

		public Guid? NullableGuid { get; set; } = Guid.NewGuid();
	}
}
