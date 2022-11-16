namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.DomainEvents;

	public class TestCrudDomainEventsFactory : ICrudDomainEventsFactory
	{
		/// <inheritdoc />
		public IAddedDomainEvent CreateAddedEvent<TAggregateRoot, TKey>(TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			return null;
		}

		/// <inheritdoc />
		public IUpdatedDomainEvent CreateUpdatedEvent<TAggregateRoot, TKey>(TAggregateRoot beforeUpdateItem, TAggregateRoot afterUpdateItem)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			return null;
		}

		/// <inheritdoc />
		public IRemovedDomainEvent CreateRemovedEvent<TAggregateRoot, TKey>(TKey id, TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			return null;
		}
	}
}
