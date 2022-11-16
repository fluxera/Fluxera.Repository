namespace Fluxera.Repository.DomainEvents
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     A domain event that indicates that a new item of <see cref="AggregateRoot{TAggregateRoot,TKey}" />
	///     has been removed to the data storage.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
	/// <typeparam name="TKey">The ID type.</typeparam>
	[PublicAPI]
	public abstract class ItemRemoved<TAggregateRoot, TKey> : IDomainEvent, IRemovedDomainEvent, ICrudDomainEvent
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ItemRemoved{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="item">The underlying item of this event.</param>
		/// <param name="id">The id of the underlying item of this event.</param>
		protected ItemRemoved(TKey id, TAggregateRoot item)
		{
			this.ID = Guard.Against.Null(id);
			this.RemovedItem = Guard.Against.Null(item);
		}

		/// <summary>
		///     Gets the id of deleted item.
		/// </summary>
		public TKey ID { get; }

		/// <summary>
		///     Gets the deleted item (has no ID anymore).
		/// </summary>
		public TAggregateRoot RemovedItem { get; }
	}
}
