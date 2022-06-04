namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	using System;
	using Fluxera.Entity;

	public class ReferenceGuidEntity : Entity<ReferenceGuidEntity, Guid>
	{
		public string Name { get; set; }
	}
}
