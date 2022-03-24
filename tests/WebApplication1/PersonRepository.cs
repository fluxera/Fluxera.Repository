namespace WebApplication1
{
	using System;
	using Fluxera.Repository;

	public class PersonRepository : Repository<Person, Guid>, IPersonRepository
	{
		/// <inheritdoc />
		public PersonRepository(IRepository<Person, Guid> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
