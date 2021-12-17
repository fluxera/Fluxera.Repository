namespace Fluxera.Repository.Validation
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IValidationStrategy<in TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		Task ValidateAsync(TAggregateRoot item);

		Task ValidateAsync(IEnumerable<TAggregateRoot> items);
	}
}
