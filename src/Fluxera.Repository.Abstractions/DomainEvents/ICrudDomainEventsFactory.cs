namespace Fluxera.Repository.DomainEvents
{
	using System;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	/// </summary>
	[PublicAPI]
	public interface ICrudDomainEventsFactory
	{
		/// <summary>
		///     Creates a new added event instance for the given aggregate root type.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		IAddedDomainEvent CreateAddedEvent<TAggregateRoot, TKey>(TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a new updated event instance for the given aggregate root type.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="beforeUpdateItem"></param>
		/// <param name="afterUpdateItem"></param>
		/// <returns></returns>
		IUpdatedDomainEvent CreateUpdatedEvent<TAggregateRoot, TKey>(TAggregateRoot beforeUpdateItem, TAggregateRoot afterUpdateItem)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>;

		/// <summary>
		///     Creates a new removed event instance for the given aggregate root type.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="id"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		IRemovedDomainEvent CreateRemovedEvent<TAggregateRoot, TKey>(TKey id, TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>;
	}
}
