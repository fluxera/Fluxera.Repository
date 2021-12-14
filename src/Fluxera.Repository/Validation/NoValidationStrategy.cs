namespace Fluxera.Repository.Validation
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity;

	internal sealed class NoValidationStrategy<TAggregateRoot> : IValidationStrategy<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		/// <inheritdoc />
		public Task ValidateAsync(TAggregateRoot item)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task ValidateAsync(IEnumerable<TAggregateRoot> items)
		{
			return Task.CompletedTask;
		}
	}
}
