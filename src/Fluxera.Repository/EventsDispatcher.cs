namespace Fluxera.Repository
{
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Utilities;
	using Microsoft.Extensions.Logging;

	internal sealed class EventsDispatcher<TAggregateRoot> : Disposable
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly ILogger logger;
		private IDomainEventDispatcher domainEventDispatcher;
		private TAggregateRoot item;

		private EventsDispatcher(TAggregateRoot item, IDomainEventDispatcher domainEventDispatcher, ILoggerFactory loggerFactory)
		{
			this.item = item;
			this.domainEventDispatcher = domainEventDispatcher;
			this.logger = loggerFactory.CreateLogger(LoggerNames.EventsDispatcher);
		}

		internal static async Task<EventsDispatcher<TAggregateRoot>> DispatchAsync(TAggregateRoot item,
			IDomainEventDispatcher domainEventDispatcher, ILoggerFactory loggerFactory)
		{
			EventsDispatcher<TAggregateRoot> dispatcher = new EventsDispatcher<TAggregateRoot>(item, domainEventDispatcher, loggerFactory);
			await dispatcher.DispatchDomainEventsAsync(false).ConfigureAwait(false);
			return dispatcher;
		}

		internal async Task CommittedDispatchAsync()
		{
			await this.DispatchDomainEventsAsync(true).ConfigureAwait(false);
		}

		protected override void DisposeManaged()
		{
			this.item = null!;
			this.domainEventDispatcher = null!;

			base.DisposeManaged();
		}

		private async Task DispatchDomainEventsAsync(bool dispatchCommitted)
		{
			string committed = dispatchCommitted ? "After commit" : "Before commit";
			this.logger.LogTrace($"Dispatching domain events ({committed}): Type = {typeof(TAggregateRoot)}, Count = {this.item.DomainEvents.Count}");

			if(dispatchCommitted)
			{
				this.item.DomainEvents.Clear();
			}

			foreach(IDomainEvent domainEvent in this.item.DomainEvents)
			{
				if(dispatchCommitted)
				{
					await this.domainEventDispatcher.DispatchCommittedAsync(domainEvent).ConfigureAwait(false);
				}
				else
				{
					await this.domainEventDispatcher.DispatchAsync(domainEvent).ConfigureAwait(false);
				}
			}
		}
	}
}
