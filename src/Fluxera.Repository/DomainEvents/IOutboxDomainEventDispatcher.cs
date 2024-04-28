namespace Fluxera.Repository.DomainEvents
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.DomainEvents.Abstractions;
	using JetBrains.Annotations;

	/// <summary>
	///     A specialized <see cref="IDomainEventDispatcher" /> that buffers the events in
	///     a queue on dispatch and flushes them all at one to the domain event handlers.
	/// </summary>
	[PublicAPI]
	public interface IOutboxDomainEventDispatcher : IDomainEventDispatcher
	{
		/// <summary>
		///     Flushes the outbox content to the domain event handlers.
		/// </summary>
		/// <returns></returns>
		Task FlushAsync(CancellationToken cancellationToken = default);

		/// <summary>
		///     Clears the outbox queue.
		/// </summary>
		void Clear();
	}
}
