namespace Fluxera.Repository.DomainEvents
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     A domain event that indicates that a new item of <see cref="AggregateRoot{TAggregateRoot,TKey}" />
	///     has been added to the data storage.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
	/// <typeparam name="TKey">The ID type.</typeparam>
	[PublicAPI]
	public abstract class ItemAdded<TAggregateRoot, TKey> : IDomainEvent, IAddedDomainEvent, ICrudDomainEvent
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ItemAdded{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="item">The underlying item of this event.</param>
		protected ItemAdded(TAggregateRoot item)
		{
			this.AddedItem = Guard.Against.Null(item);
		}

		/// <summary>
		///     Gets the added item.
		/// </summary>
		public TAggregateRoot AddedItem { get; }
	}
}
