namespace Fluxera.Repository.UnitTests.PersonAggregate
{
	using Fluxera.Repository.Traits;

	public interface IPersonRepository : ICanGet<Person>
	{
	}
}
