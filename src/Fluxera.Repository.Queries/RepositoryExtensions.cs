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

	/// <summary>
	///		Extension methods for use with the <see cref="IReadOnlyRepository{T, TKey}"/>.
	/// </summary>
	[PublicAPI]
	public static class RepositoryExtensions
	{
		/// <summary>
		///		Executes the get query defined by the given <typeparamref name="TKey"/> and <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repository"></param>
		/// <param name="id"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<SingleResult> ExecuteGetAsync<T, TKey>(this IReadOnlyRepository<T, TKey> repository,
			TKey id,
			QueryOptions queryOptions,
			CancellationToken cancellationToken = default)
			where T : AggregateRoot<T, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(id);
			Guard.Against.Null(repository);
			Guard.Against.Null(queryOptions);

			// 1. Build the selector expression (optional).
			Expression<Func<T, T>> selector = queryOptions.ToSelector<T>();

			// 2. Execute the get query.
			T item = selector is null
				? await repository.GetAsync(id, cancellationToken)
				: await repository.GetAsync(id, selector, cancellationToken);

			return new SingleResult(item);
		}

		/// <summary>
		///		Executes the count query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repository"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<long> ExecuteCountAsync<T, TKey>(this IReadOnlyRepository<T, TKey> repository,
			QueryOptions queryOptions,
			CancellationToken cancellationToken = default)
			where T : AggregateRoot<T, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(repository);
			Guard.Against.Null(queryOptions);

			// 1. Build the query predicate.
			Expression<Func<T, bool>> predicate = queryOptions.ToPredicate<T>();

			// 2. Execute the count query.
			long count = await repository.CountAsync(predicate, cancellationToken);

			return count;
		}

		/// <summary>
		///		Executes the find many query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repository"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<QueryResult> ExecuteFindManyAsync<T, TKey>(this IReadOnlyRepository<T, TKey> repository,
			QueryOptions queryOptions,
			CancellationToken cancellationToken = default)
			where T : AggregateRoot<T, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(repository);
			Guard.Against.Null(queryOptions);

			// 1. Build the query options: sorting and paging.
			IQueryOptions<T> options = queryOptions.ToQueryOptions<T>();

			// 2. Build the query predicate.
			Expression<Func<T, bool>> predicate = queryOptions.ToPredicate<T>();

			// 3. Build the selector expression (optional).
			Expression<Func<T, T>> selector = queryOptions.ToSelector<T>();

			// 4. Get the total count of the query (optional).
			long? totalCount = null;
			if(queryOptions.Count is not null)
			{
				if(queryOptions.Count.CountValue)
				{
					totalCount = await repository.CountAsync(predicate, cancellationToken);
				}
			}

			// 5. Execute the find many query.
			IReadOnlyCollection<T> items = selector is null
				? await repository.FindManyAsync(predicate, options, cancellationToken)
				: await repository.FindManyAsync(predicate, selector, options, cancellationToken);

			return new QueryResult(items, totalCount);
		}

		/// <summary>
		///		Executes the find one query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repository"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<T> ExecuteFindOneAsync<T, TKey>(this IReadOnlyRepository<T, TKey> repository,
			QueryOptions queryOptions,
			CancellationToken cancellationToken = default)
			where T : AggregateRoot<T, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(repository);
			Guard.Against.Null(queryOptions);

			// 1. Build the query options: sorting and paging.
			IQueryOptions<T> options = queryOptions.ToQueryOptions<T>();

			// 2. Build the query predicate.
			Expression<Func<T, bool>> predicate = queryOptions.ToPredicate<T>();

			// 3. Build the selector expression (optional).
			Expression<Func<T, T>> selector = queryOptions.ToSelector<T>();

			// 4. Execute the find many query.
			T item = selector is null
				? await repository.FindOneAsync(predicate, options, cancellationToken)
				: await repository.FindOneAsync(predicate, selector, options, cancellationToken);

			return item;
		}
	}
}
