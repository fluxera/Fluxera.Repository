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
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;

	public class DomainEventsTestRepository<TAggregateRoot, TKey> : NoopTestRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <inheritdoc />
		public override async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			Person[] persons =
			{
				new Person
				{
					ID = "1",
					Name = "Tester"
				},
				new Person
				{
					ID = "2",
					Name = "Tester"
				},
				new Person
				{
					ID = "3",
					Name = "Tester"
				}
			};
			IReadOnlyCollection<TAggregateRoot> items = persons.Cast<TAggregateRoot>().AsReadOnly();
			return items;
		}

		/// <inheritdoc />
		public override async Task<TAggregateRoot> GetAsync(TKey id, CancellationToken cancellationToken)
		{
			Person item = new Person
			{
				ID = "3",
				Name = "Tester"
			};
			return item as TAggregateRoot;
		}
	}
}
