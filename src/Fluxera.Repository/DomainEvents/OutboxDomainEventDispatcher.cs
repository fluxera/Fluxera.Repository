namespace Fluxera.Repository.DomainEvents
{
	using Fluxera.DomainEvents;
	using Fluxera.DomainEvents.Abstractions;
	using JetBrains.Annotations;
	using Mediator;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	///     A specialized <see cref="MediatorDomainEventDispatcher" /> that buffers the events in
	///     a queue on dispatch and flushes them all at one to the domain event handlers.
	/// </summary>
	[PublicAPI]
	internal sealed class OutboxDomainEventDispatcher : MediatorDomainEventDispatcher, IOutboxDomainEventDispatcher
	{
		private readonly IEnumerable<IDomainEventsReducer> reducers;

		private readonly ConcurrentQueue<IDomainEvent> outbox = new ConcurrentQueue<IDomainEvent>();

		/// <inheritdoc />
		public OutboxDomainEventDispatcher(IPublisher publisher, IEnumerable<IDomainEventsReducer> reducers)
			: base(publisher)
		{
			this.reducers = reducers;
		}

		/// <inheritdoc />
		public override Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
		{
			this.outbox.Enqueue(domainEvent);

			return Task.CompletedTask;
		}

		/// <summary>
		///     Flushes the outbox context to the domain event handlers.
		/// </summary>
		/// <returns></returns>
		public async Task FlushAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				IReadOnlyCollection<IDomainEvent> domainEvents = this.outbox;

				foreach(IDomainEventsReducer domainEventsReducer in this.reducers)
				{
					domainEvents = domainEventsReducer.Reduce(domainEvents);
				}

				foreach(IDomainEvent domainEvent in domainEvents)
				{
					await base.DispatchAsync(domainEvent, cancellationToken);
				}
			}
			finally
			{
				this.Clear();
			}
		}

		/// <summary>
		///     Clears the outbox queue.
		/// </summary>
		public void Clear()
		{
			this.outbox.Clear();
		}
	}
}
