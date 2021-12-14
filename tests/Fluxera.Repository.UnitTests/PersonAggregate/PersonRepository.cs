namespace Fluxera.Repository.UnitTests.PersonAggregate
{
	public class PersonRepository : Repository<Person>, IPersonRepository
	{
		/// <inheritdoc />
		public PersonRepository(IRepository<Person> innerRepository) 
			: base(innerRepository)
		{
		}
	}
}
