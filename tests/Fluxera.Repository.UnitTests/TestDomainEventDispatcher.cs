namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;

	public class TestDomainEventDispatcher : IDomainEventDispatcher
	{
		/// <inheritdoc />
		public async Task DispatchAsync(dynamic domainEvent)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public async Task DispatchCommittedAsync(dynamic domainEvent)
		{
			throw new NotImplementedException();
		}
	}
}
