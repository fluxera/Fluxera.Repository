namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;

	/// <summary>
	///     A specialized <see cref="DomainEventDispatcher" /> that buffers the events in
	///     a queue on dispatch and flushes them all at one to the domain event handlers.
	/// </summary>
	[PublicAPI]
	public sealed class OutboxDomainEventDispatcher : DomainEventDispatcher
	{
		private readonly IEnumerable<IDomainEventsReducer> reducers;

		private readonly ConcurrentQueue<IDomainEvent> outbox = new ConcurrentQueue<IDomainEvent>();

		/// <inheritdoc />
		public OutboxDomainEventDispatcher(IServiceProvider serviceProvider, IEnumerable<IDomainEventsReducer> reducers)
			: base(serviceProvider)
		{
			this.reducers = reducers;
		}

		/// <inheritdoc />
		public override Task DispatchAsync(IDomainEvent domainEvent)
		{
			this.outbox.Enqueue(domainEvent);

			return Task.CompletedTask;
		}

		/// <summary>
		///     Flushes the outbox context to the domain event handlers.
		/// </summary>
		/// <returns></returns>
		public async Task FlushAsync()
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
					await base.DispatchAsync(domainEvent);
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
