﻿#pragma warning disable CA2021
namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;

	public class DomainEventsTestRepository<TEntity, TKey> : NoopTestRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		public override Task<IReadOnlyCollection<TEntity>> FindManyAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions = null, CancellationToken cancellationToken = default)
		{
			Person[] persons =
			[
				new Person
				{
					ID = Guid.Parse("8693cbd0-a564-47cf-9fe3-b1444392957d"),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.Parse("c8fbfccd-a14c-41ba-8e2f-d32b286b6804"),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.Parse("fabb0b65-45c5-4aff-87b0-45b766074588"),
					Name = "Tester"
				}
			];
			IReadOnlyCollection<TEntity> items = persons.Cast<TEntity>().AsReadOnly();
			return Task.FromResult(items);
		}

		/// <inheritdoc />
		public override Task<IReadOnlyCollection<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			Person[] persons =
			[
				new Person
				{
					ID = Guid.Parse("8693cbd0-a564-47cf-9fe3-b1444392957d"),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.Parse("c8fbfccd-a14c-41ba-8e2f-d32b286b6804"),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.Parse("fabb0b65-45c5-4aff-87b0-45b766074588"),
					Name = "Tester"
				}
			];
			IReadOnlyCollection<TEntity> items = persons.Cast<TEntity>().AsReadOnly();
			return Task.FromResult(items);
		}

		/// <inheritdoc />
		public override Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken)
		{
			Person item = new Person
			{
				ID = Guid.Parse("d37eaa47-7cb0-4368-af1a-8f1c94be9782"),
				Name = "Tester"
			};
			return Task.FromResult(item as TEntity);
		}
	}
}
