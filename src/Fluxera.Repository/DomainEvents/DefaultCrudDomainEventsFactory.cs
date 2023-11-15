namespace Fluxera.Repository.DomainEvents
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Entity;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	/// <summary>
	///     A reflection-based, convention-based, opinionated CRUD domain event factory.
	/// </summary>
	[UsedImplicitly]
	internal sealed class DefaultCrudDomainEventsFactory : ICrudDomainEventsFactory
	{
		private readonly ConcurrentDictionary<string, Type> typeCache = new ConcurrentDictionary<string, Type>();

		public DefaultCrudDomainEventsFactory()
		{
			IEnumerable<Type> types = AppDomain
				.CurrentDomain
				.GetAssemblies()
				.SelectMany(assembly => assembly
					.GetExportedTypes()
					.Where(type => !type.IsAbstract && !type.IsInterface && type.IsAssignableTo<ICrudDomainEvent>()));

			foreach(Type type in types)
			{
				if(!this.typeCache.TryAdd(type.Name, type))
				{
					throw new InvalidOperationException($"Duplicate CRUD domain event found: '{type.Name}'.");
				}
			}
		}

		/// <inheritdoc />
		public IAddedDomainEvent CreateAddedEvent<TAggregateRoot, TKey>(TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			string eventTypeName = $"{typeof(TAggregateRoot).Name}Added";

			return this.CreateEventInstance<ItemAdded<TAggregateRoot, TKey>>(eventTypeName, item);
		}

		/// <inheritdoc />
		public IUpdatedDomainEvent CreateUpdatedEvent<TAggregateRoot, TKey>(TAggregateRoot beforeUpdateItem, TAggregateRoot afterUpdateItem)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			string eventTypeName = $"{typeof(TAggregateRoot).Name}Updated";

			return this.CreateEventInstance<ItemUpdated<TAggregateRoot, TKey>>(eventTypeName, beforeUpdateItem, afterUpdateItem);
		}

		/// <inheritdoc />
		public IRemovedDomainEvent CreateRemovedEvent<TAggregateRoot, TKey>(TKey id, TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			string eventTypeName = $"{typeof(TAggregateRoot).Name}Removed";

			return this.CreateEventInstance<ItemRemoved<TAggregateRoot, TKey>>(eventTypeName, id, item);
		}

		private TEvent CreateEventInstance<TEvent>(string eventTypeName, params object[] args)
			where TEvent : class, IDomainEvent
		{
			TEvent domainEvent = null;
			if(this.typeCache.TryGetValue(eventTypeName, out Type eventType))
			{
				domainEvent = (TEvent)Activator.CreateInstance(eventType, args);
			}

			return domainEvent;
		}
	}
}
