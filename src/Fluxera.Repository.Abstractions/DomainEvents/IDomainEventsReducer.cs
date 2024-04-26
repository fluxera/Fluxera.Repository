namespace Fluxera.Repository.DomainEvents
{
	using System.Collections.Generic;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a reducer that takes the raised domain events and reduces
	///     this list by its custom rules and returns the new list of domain events
	///     that are then dispatched to their handlers.
	/// </summary>
	[PublicAPI]
	public interface IDomainEventsReducer
	{
		/// <summary>
		///     Reduces the given list of domain events.
		/// </summary>
		/// <param name="domainEvents"></param>
		/// <returns></returns>
		IReadOnlyCollection<IDomainEvent> Reduce(IReadOnlyCollection<IDomainEvent> domainEvents);
	}
}
