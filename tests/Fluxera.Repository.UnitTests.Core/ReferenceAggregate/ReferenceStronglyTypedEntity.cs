namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	using Fluxera.Entity;

	public class ReferenceStronglyTypedEntity : Entity<ReferenceStronglyTypedEntity, ReferenceStronglyTypedEntityId>
	{
		public string Name { get; set; }
	}
}
