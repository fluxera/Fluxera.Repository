namespace Fluxera.Repository.Queries
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using AutoMapper;
	using AutoMapper.Extensions.ExpressionMapping;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Queries;
	using Fluxera.Queries.Expressions;
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
		///		Executes the find many query defined by the given <see cref="QueryOptions" />.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TDto"></typeparam>
		/// <param name="repository"></param>
		/// <param name="queryOptions"></param>
		/// <param name="mapper"></param>
		/// <param name="queryOptionsBuilder"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<QueryResult> ExecuteFindManyAsync<TAggregateRoot, TKey, TDto>(this IReadOnlyRepository<TAggregateRoot, TKey> repository,
			QueryOptions queryOptions,
			IMapper mapper,
			IQueryOptionsBuilder<TAggregateRoot> queryOptionsBuilder,
			CancellationToken cancellationToken = default)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
			where TDto : class
		{
			Guard.Against.Null(queryOptions);

			// 1. Build the query options: sorting and paging.
			IReadOnlyCollection<OrderByExpression<TDto>> orderBy = queryOptions.ToOrderBy<TDto>();
			IList<OrderByExpression<TAggregateRoot>> mappedOrderBy = new List<OrderByExpression<TAggregateRoot>>();

			foreach(OrderByExpression<TDto> orderByExpression in orderBy)
			{
				Expression<Func<TAggregateRoot, object>> mappedSelectorExpression = mapper.Map<Expression<Func<TAggregateRoot, object>>>(orderByExpression.SelectorExpression);
				OrderByExpression<TAggregateRoot> mappedOrderByExpression = new OrderByExpression<TAggregateRoot>(mappedSelectorExpression, orderByExpression.Direction);
				mappedOrderBy.Add(mappedOrderByExpression);
			}

			IQueryOptions<TAggregateRoot> mappedOptions = queryOptionsBuilder
				.Apply(mappedOrderBy.AsReadOnly())
				.Apply(queryOptions.Skip, queryOptions.Top)
				.Build();

			// 2. Build the query predicate.
			Expression<Func<TDto, bool>> predicate = queryOptions.ToPredicate<TDto>();
			Expression<Func<TAggregateRoot, bool>> mappedPredicate = mapper.MapExpression<Expression<Func<TAggregateRoot, bool>>>(predicate);

			// 3. Build the selector expression (optional).
			Expression<Func<TDto, TDto>> selector = queryOptions.ToSelector<TDto>();
			Expression<Func<TAggregateRoot, TAggregateRoot>> mappedSelector = mapper.MapExpression<Expression<Func<TAggregateRoot, TAggregateRoot>>>(selector);

			// 4. Get the total count of the query (optional).
			long? totalCount = null;
			if(queryOptions.Count is not null)
			{
				if(queryOptions.Count.CountValue)
				{
					totalCount = await repository.CountAsync(mappedPredicate, cancellationToken);
				}
			}

			// 5. Execute the find many query.
			IReadOnlyCollection<TAggregateRoot> items = selector is null
				? await repository.FindManyAsync(mappedPredicate, mappedOptions, cancellationToken)
				: await repository.FindManyAsync(mappedPredicate, mappedSelector, mappedOptions, cancellationToken);

			return new QueryResult(items, totalCount);
		}

		///  <summary>
		/// 	Executes the get query defined by the given ID and <see cref="QueryOptions" />.
		///  </summary>
		///  <typeparam name="TAggregateRoot"></typeparam>
		///  <typeparam name="TKey"></typeparam>
		///  <typeparam name="TDto"></typeparam>
		///  <param name="repository"></param>
		///  <param name="id"></param>
		///  <param name="queryOptions"></param>
		///  <param name="mapper"></param>
		///  <param name="cancellationToken"></param>
		///  <returns></returns>
		public static async Task<SingleResult> ExecuteGetAsync<TAggregateRoot, TKey, TDto>(this IReadOnlyRepository<TAggregateRoot, TKey> repository,
			TKey id,
			QueryOptions queryOptions,
			IMapper mapper,
			CancellationToken cancellationToken = default)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
			where TDto : class
		{
			Guard.Against.Null(id);
			Guard.Against.Null(queryOptions);

			// 1. Build the selector expression (optional).
			Expression<Func<TDto, TDto>> selector = queryOptions.ToSelector<TDto>();
			Expression<Func<TAggregateRoot, TAggregateRoot>> mappedSelector = mapper.MapExpression<Expression<Func<TAggregateRoot, TAggregateRoot>>>(selector);

			// 2. Execute the get query.
			TAggregateRoot item = selector is null
				? await repository.GetAsync(id, cancellationToken)
				: await repository.GetAsync(id, mappedSelector, cancellationToken);

			return new SingleResult(item);
		}

		/// <summary>
		///		Executes the count query defined by the given <see cref="QueryOptions" />.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TDto"></typeparam>
		/// <param name="repository"></param>
		/// <param name="queryOptions"></param>
		/// <param name="mapper"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<long> ExecuteCountAsync<TAggregateRoot, TKey, TDto>(this IReadOnlyRepository<TAggregateRoot, TKey> repository,
			QueryOptions queryOptions,
			IMapper mapper,
			CancellationToken cancellationToken = default)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
			where TDto : class
		{
			Guard.Against.Null(queryOptions);

			// 1. Build the query predicate.
			Expression<Func<TDto, bool>> predicate = queryOptions.ToPredicate<TDto>();
			Expression<Func<TAggregateRoot, bool>> mappedPredicate = mapper.MapExpression<Expression<Func<TAggregateRoot, bool>>>(predicate);

			// 2. Execute the count query.
			long count = await repository.CountAsync(mappedPredicate, cancellationToken);

			return count;
		}

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
