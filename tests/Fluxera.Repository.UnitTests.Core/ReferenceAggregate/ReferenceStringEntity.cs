namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	using Fluxera.Entity;

	public class ReferenceStringEntity : Entity<ReferenceStringEntity, string>
	{
		public string Name { get; set; }
	}
}
