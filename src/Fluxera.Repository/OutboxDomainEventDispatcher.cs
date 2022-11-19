namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	/// <summary>
	///     A specialized <see cref="DomainEventDispatcher" /> that buffers the events in
	///     a queue on dispatch and flushes them all at one to the domain event handlers.
	/// </summary>
	[PublicAPI]
	public sealed class OutboxDomainEventDispatcher : DomainEventDispatcher
	{
		private readonly ConcurrentQueue<IDomainEvent> outbox = new ConcurrentQueue<IDomainEvent>();

		/// <inheritdoc />
		public OutboxDomainEventDispatcher(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
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
				foreach(IDomainEvent domainEvent in this.outbox)
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
