namespace Fluxera.Repository.Queries
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Queries;
	using Fluxera.Queries.Options;
	using Fluxera.Repository;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;

	/// <inheritdoc />
	[UsedImplicitly]
	internal sealed class RepositoryQueryExecutor<TEntity, TKey> : QueryExecutorBase<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IReadOnlyRepository<TEntity, TKey> repository;

		public RepositoryQueryExecutor(IReadOnlyRepository<TEntity, TKey> repository)
		{
			this.repository = repository;
		}

		/// <inheritdoc />
		public override async Task<QueryResult> ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			Guard.Against.Null(queryOptions);

			// 1. Build the query options: sorting and paging.
			IQueryOptions<TEntity> options = queryOptions.ToQueryOptions<TEntity>();

			// 2. Build the query predicate.
			Expression<Func<TEntity, bool>> predicate = queryOptions.ToPredicate<TEntity>();

			// 3. Build the selector expression (optional).
			Expression<Func<TEntity, TEntity>> selector = queryOptions.ToSelector<TEntity>();

			// 4. Get the total count of the query (optional).
			long? totalCount = null;
			if(queryOptions.Count is not null)
			{
				if(queryOptions.Count.CountValue)
				{
					totalCount = await this.repository.CountAsync(predicate, cancellationToken);
				}
			}

			// 5. Execute the find many query.
			IReadOnlyCollection<TEntity> items = selector is null
				? await this.repository.FindManyAsync(predicate, options, cancellationToken)
				: await this.repository.FindManyAsync(predicate, selector, options, cancellationToken);

			return new QueryResult(items, totalCount);
		}

		/// <inheritdoc />
		public override async Task<SingleResult> ExecuteGetAsync(TKey id, QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			Guard.Against.Null(id);
			Guard.Against.Null(queryOptions);

			// 1. Build the selector expression (optional).
			Expression<Func<TEntity, TEntity>> selector = queryOptions.ToSelector<TEntity>();

			// 2. Execute the get query.
			TEntity item = selector is null
				? await this.repository.GetAsync(id, cancellationToken)
				: await this.repository.GetAsync(id, selector, cancellationToken);

			return new SingleResult(item);
		}

		/// <inheritdoc />
		public override async Task<long> ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			Guard.Against.Null(queryOptions);

			// 1. Build the query predicate.
			Expression<Func<TEntity, bool>> predicate = queryOptions.ToPredicate<TEntity>();

			// 2. Execute the count query.
			long count = await this.repository.CountAsync(predicate, cancellationToken);

			return count;
		}
	}
}
