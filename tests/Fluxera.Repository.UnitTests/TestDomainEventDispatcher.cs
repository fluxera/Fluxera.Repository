namespace Fluxera.Repository.UnitTests
{
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;

	public class TestDomainEventDispatcher : IDomainEventDispatcher
	{
		public bool DispatchCommittedWasCalled;
		public bool DispatchWasCalled;

		/// <inheritdoc />
		public async Task DispatchAsync(dynamic domainEvent)
		{
			this.DispatchWasCalled = true;
		}

		/// <inheritdoc />
		public async Task DispatchCommittedAsync(dynamic domainEvent)
		{
			this.DispatchCommittedWasCalled = true;
		}
	}
}
