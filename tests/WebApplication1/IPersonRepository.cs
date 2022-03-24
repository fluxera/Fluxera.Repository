namespace WebApplication1
{
	using System;
	using Fluxera.Repository;

	public interface IPersonRepository : IRepository<Person, Guid>
	{
	}
}
