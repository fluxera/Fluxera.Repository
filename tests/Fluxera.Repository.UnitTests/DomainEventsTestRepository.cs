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

	public class DomainEventsTestRepository<T> : NoopTestRepository<T> where T : AggregateRoot<T>
	{
		/// <inheritdoc />
		public override async Task<IReadOnlyCollection<T>> FindManyAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
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
			IReadOnlyCollection<T> items = persons.Cast<T>().AsReadOnly();
			return items;
		}

		/// <inheritdoc />
		public override async Task<T> GetAsync(string id, CancellationToken cancellationToken)
		{
			Person item = new Person
			{
				ID = "3",
				Name = "Tester"
			};
			return item as T;
		}
	}
}
