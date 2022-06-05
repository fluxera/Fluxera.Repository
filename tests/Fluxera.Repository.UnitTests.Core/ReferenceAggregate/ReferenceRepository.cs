namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	public class ReferenceRepository : Repository<Reference, string>, IReferenceRepository
	{
		/// <inheritdoc />
		public ReferenceRepository(IRepository<Reference, string> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
