namespace Sample.API
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;
	using Sample.Domain.Company;
	using Sample.Domain.Company.Events;

	[UsedImplicitly]
	internal sealed class SampleCrudDomainEventsFactory : ICrudDomainEventsFactory
	{
		/// <inheritdoc />
		public IAddedDomainEvent CreateAddedEvent<TAggregateRoot, TKey>(TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			if(typeof(TAggregateRoot) == typeof(Company))
			{
				Company company = item as Company;
				return new CompanyAdded(company);
			}

			return null;
		}

		/// <inheritdoc />
		public IUpdatedDomainEvent CreateUpdatedEvent<TAggregateRoot, TKey>(TAggregateRoot beforeUpdateItem, TAggregateRoot afterUpdateItem)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			if(typeof(TAggregateRoot) == typeof(Company))
			{
				Company companyBefore = beforeUpdateItem as Company;
				Company companyAfter = afterUpdateItem as Company;
				return new CompanyUpdated(companyBefore, companyAfter);
			}

			return null;
		}

		/// <inheritdoc />
		public IRemovedDomainEvent CreateRemovedEvent<TAggregateRoot, TKey>(TKey id, TAggregateRoot item)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			if(typeof(TAggregateRoot) == typeof(Company))
			{
				CompanyId companyId = id as CompanyId;
				Company company = item as Company;
				return new CompanyRemoved(companyId, company);
			}

			return null;
		}
	}
}
