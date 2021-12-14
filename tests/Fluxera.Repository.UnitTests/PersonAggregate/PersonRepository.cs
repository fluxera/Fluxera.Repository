namespace Fluxera.Repository.UnitTests.PersonAggregate
{
	using Fluxera.Repository;

	public class PersonRepository : Repository<Person>, IPersonRepository
	{
		/// <inheritdoc />
		public PersonRepository(IRepository<Person> innerRepository) 
			: base(innerRepository)
		{
		}
	}
}
