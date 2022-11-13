namespace Fluxera.Repository.UnitTests
{
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;

	public class TestDomainEventDispatcher : IDomainEventDispatcher
	{
		public bool DispatchCommittedWasCalled;
		public bool DispatchWasCalled;

		/// <inheritdoc />
		public Task DispatchAsync(IDomainEvent domainEvent)
		{
			this.DispatchWasCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task DispatchCommittedAsync(IDomainEvent domainEvent)
		{
			this.DispatchCommittedWasCalled = true;
			return Task.CompletedTask;
		}
	}
}
