namespace Fluxera.Repository.Validation
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IValidationStrategy<in TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		Task ValidateAsync(TAggregateRoot item);

		Task ValidateAsync(IEnumerable<TAggregateRoot> items);
	}
}
