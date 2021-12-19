namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System;

	public class PersonRepository : Repository<Person, Guid>, IPersonRepository
	{
		/// <inheritdoc />
		public PersonRepository(IRepository<Person, Guid> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
