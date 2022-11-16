namespace Fluxera.Repository.DomainEvents
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     A domain event that indicates that a new item of <see cref="AggregateRoot{TAggregateRoot,TKey}" />
	///     has been updated in the data storage.
	/// </summary>
	/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
	/// <typeparam name="TKey">The ID type.</typeparam>
	[PublicAPI]
	public abstract class ItemUpdated<TAggregateRoot, TKey> : IDomainEvent, IUpdatedDomainEvent, ICrudDomainEvent
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ItemUpdated{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="beforeUpdateItem">The underlying item of this event.</param>
		/// <param name="afterUpdateItem">The underlying item of this event.</param>
		protected ItemUpdated(TAggregateRoot beforeUpdateItem, TAggregateRoot afterUpdateItem)
		{
			this.BeforeUpdateItem = Guard.Against.Null(beforeUpdateItem);
			this.AfterUpdateItem = Guard.Against.Null(afterUpdateItem);
		}

		/// <summary>
		///     Gets the item before the update.
		/// </summary>
		public TAggregateRoot BeforeUpdateItem { get; }

		/// <summary>
		///     Gets the updated item.
		/// </summary>
		public TAggregateRoot AfterUpdateItem { get; }
	}
}
