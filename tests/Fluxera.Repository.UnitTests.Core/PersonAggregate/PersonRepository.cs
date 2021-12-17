namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	public class PersonRepository : Repository<Person, string>, IPersonRepository
	{
		/// <inheritdoc />
		public PersonRepository(IRepository<Person, string> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
