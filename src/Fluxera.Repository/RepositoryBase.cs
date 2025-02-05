namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Query.Impl;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities;
	using JetBrains.Annotations;

	/// <summary>
	///     A base class for storage repository implementations that do not support LINQ or
	///     async extensions methods for <see cref="IQueryable{TEntity}" />. The base class is prepared
	///     to make implementing storage implementations easier and streamlined.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public abstract class RepositoryBase<TEntity, TKey> : Disposable, IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="RepositoryBase{TAggregateRoot, TKey}" /> type.
		/// </summary>
		/// <param name="repositoryRegistry"></param>
		protected RepositoryBase(IRepositoryRegistry repositoryRegistry)
		{
			this.RepositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => base.IsDisposed;

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.AddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(this.CreatePrimaryKeySpecification(item.ID), cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(this.CreatePrimaryKeySpecification(id), cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(new Specification<TEntity>(predicate), cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.UpdateRangeAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(new Specification<TEntity>(predicate), queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(new Specification<TEntity>(predicate), selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(new Specification<TEntity>(predicate), queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(new Specification<TEntity>(predicate), selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(this.CreatePrimaryKeySpecification(id), QueryOptions<TEntity>.Empty(), cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector,
			CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(this.CreatePrimaryKeySpecification(id), selector, QueryOptions<TEntity>.Empty(), cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(Specification<TEntity>.All, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(new Specification<TEntity>(predicate), cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TEntity>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TEntity>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(this.CreatePrimaryKeySpecification(id), cancellationToken) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(new Specification<TEntity>(predicate), cancellationToken) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(specification, cancellationToken) > 0;
		}

		/// <summary>
		///     Adds an item to the underlying storage.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task AddAsync(TEntity item, CancellationToken cancellationToken);

		/// <summary>
		///     Adds the items to the underlying store.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken);

		/// <summary>
		///     Removes the items that satisfy the specification from the underlying store.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);

		/// <summary>
		///     Removes the items from the underlying store.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken);

		/// <summary>
		///     Update the item in the underlying store.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task UpdateAsync(TEntity item, CancellationToken cancellationToken);

		/// <summary>
		///     Updates the items in the underlying store.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken);

		/// <summary>
		///     Finds the first (or none) items that satisfies the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<TEntity> FindOneAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions,
			CancellationToken cancellationToken);

		/// <summary>
		///     Finds the first (or none) items that satisfies the specification and returns the selected value.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<TResult> FindOneAsync<TResult>(ISpecification<TEntity> specification, Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Finds many (or none) items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<IReadOnlyCollection<TEntity>> FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Finds many (or none) items that satisfy the specification and returns the selected value for each item.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the count of items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> LongCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<int> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<int> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken);

		/// <summary>
		///     Creates an <see cref="Expression" /> in the form of <c>x => x.ID == id</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected virtual Expression<Func<TEntity, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			return id.CreatePrimaryKeyPredicate<TEntity, TKey>();
		}

		/// <summary>
		///     Creates a <see cref="ISpecification{T}" /> in the form of <c>x => x.ID == id</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected ISpecification<TEntity> CreatePrimaryKeySpecification(TKey id)
		{
			Expression<Func<TEntity, bool>> predicate = this.CreatePrimaryKeyPredicate(id);
			ISpecification<TEntity> specification = new Specification<TEntity>(predicate);
			return specification;
		}
	}
}
