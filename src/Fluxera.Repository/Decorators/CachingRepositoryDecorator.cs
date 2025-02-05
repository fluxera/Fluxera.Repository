namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;

	/// <summary>
	///     A repository decorator that controls teh caching feature.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class CachingRepositoryDecorator<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ICachingStrategy<TEntity, TKey> cachingStrategy;
		private readonly IRepository<TEntity, TKey> innerRepository;

		/// <summary>
		///     Creates a new instance of the <see cref="CachingRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="cachingStrategyFactory"></param>
		public CachingRepositoryDecorator(
			IRepository<TEntity, TKey> innerRepository,
			ICachingStrategyFactory cachingStrategyFactory)
		{
			this.innerRepository = Guard.Against.Null(innerRepository);
			Guard.Against.Null(cachingStrategyFactory);

			this.cachingStrategy = cachingStrategyFactory.CreateStrategy<TEntity, TKey>();
		}

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.AddAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IEnumerable<TEntity> itemsList = items.ToList();

			await this.innerRepository.AddRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.AddAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.UpdateAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IEnumerable<TEntity> itemsList = items.ToList();

			await this.innerRepository.UpdateRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.UpdateAsync(itemsList).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			TKey id = item.ID;

			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TEntity> items = await this.innerRepository.FindManyAsync(predicate, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
			IReadOnlyCollection<TKey> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<TEntity> items = await this.innerRepository.FindManyAsync(specification, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
			IReadOnlyCollection<TKey> ids = items.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IEnumerable<TEntity> itemsList = items.ToList();

			IReadOnlyCollection<TKey> ids = itemsList.Select(x => x.ID).AsReadOnly();

			await this.innerRepository.RemoveRangeAsync(itemsList, cancellationToken).ConfigureAwait(false);

			await this.cachingStrategy.RemoveAsync(ids).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.GetAsync(id,
					async () => await this.innerRepository.GetAsync(id, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(true);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.GetAsync(id, selector,
					async () => await this.innerRepository.GetAsync(id, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(id,
					async () => await this.innerRepository.ExistsAsync(id, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(predicate, queryOptions,
					async () => await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(specification.Predicate, queryOptions,
					async () => await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(predicate, selector, queryOptions,
					async () => await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindOneAsync(specification.Predicate, selector, queryOptions,
					async () => await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(predicate,
					async () => await this.innerRepository.ExistsAsync(predicate, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.ExistsAsync(specification.Predicate,
					async () => await this.innerRepository.ExistsAsync(specification, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(predicate, queryOptions,
					async () => await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(specification.Predicate, queryOptions,
					async () => await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(predicate, selector, queryOptions,
					async () => await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.FindManyAsync(specification.Predicate, selector, queryOptions,
					async () => await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(
					async () => await this.innerRepository.CountAsync(cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(predicate,
					async () => await this.innerRepository.CountAsync(predicate, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.CountAsync(specification.Predicate,
					async () => await this.innerRepository.CountAsync(specification, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(
					async () => await this.innerRepository.SumAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(predicate,
					async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.SumAsync(specification.Predicate,
					async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(
					async () => await this.innerRepository.AverageAsync(selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(predicate,
					async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.cachingStrategy.AverageAsync(specification.Predicate,
					async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken)
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			if(!this.innerRepository.IsDisposed)
			{
				this.innerRepository.Dispose();
			}
		}

		/// <inheritdoc />
		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if(!this.innerRepository.IsDisposed)
			{
				await this.innerRepository.DisposeAsync();
			}
		}

		/// <inheritdoc />
		public RepositoryName RepositoryName => this.innerRepository.RepositoryName;

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
