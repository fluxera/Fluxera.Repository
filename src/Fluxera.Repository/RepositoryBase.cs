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
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities;
	using JetBrains.Annotations;

	/// <summary>
	///     A base class for storage repository implementations that do not support LINQ or
	///     async extensions methods for <see cref="IQueryable{T}" />. The base class is prepared
	///     to make implementing storage implementations easier and streamlined.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public abstract class RepositoryBase<TAggregateRoot, TKey> : Disposable, IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => base.IsDisposed;

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.AddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(this.CreatePrimaryKeySpecification(item.ID), cancellationToken).ConfigureAwait(false);
			// ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
			item.ID = default(TKey);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(this.CreatePrimaryKeySpecification(id), cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(new Specification<TAggregateRoot>(predicate), cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IEnumerable<TAggregateRoot> itemsList = items.ToList();
			await this.RemoveRangeAsync(itemsList, cancellationToken);
			foreach(TAggregateRoot item in itemsList)
			{
				// ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
				item.ID = default(TKey);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.UpdateRangeAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(new Specification<TAggregateRoot>(predicate), queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(new Specification<TAggregateRoot>(predicate), selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(new Specification<TAggregateRoot>(predicate), queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(new Specification<TAggregateRoot>(predicate), selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(this.CreatePrimaryKeySpecification(id), QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(this.CreatePrimaryKeySpecification(id), selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(Specification<TAggregateRoot>.All, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(new Specification<TAggregateRoot>(predicate), cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(Specification<TAggregateRoot>.All, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(new Specification<TAggregateRoot>(predicate), selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TAggregateRoot, TKey>.AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(this.CreatePrimaryKeySpecification(id), cancellationToken) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(new Specification<TAggregateRoot>(predicate), cancellationToken) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(specification, cancellationToken) > 0;
		}

		/// <summary>
		///     Adds an item to the underlying storage.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken);

		/// <summary>
		///     Adds the items to the underlying store.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken);

		/// <summary>
		///     Removes the items that satisfy the specification from the underlying store.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken);

		/// <summary>
		///     Removes the items from the underlying store.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken);

		/// <summary>
		///     Update the item in the underlying store.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken);

		/// <summary>
		///     Updates the items in the underlying store.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken);

		/// <summary>
		///     Finds the first (or none) items that satisfies the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Finds the first (or none) items that satisfies the specification and returns the selected value.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Finds many (or none) items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Finds many (or none) items that satisfy the specification and returns the selected value for each item.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the count of items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the sum of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the average of the selected value for items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken);

		/// <summary>
		///     Creates an <see cref="Expression" /> in the form of <c>x => x.ID == id</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected virtual Expression<Func<TAggregateRoot, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			return id.CreatePrimaryKeyPredicate<TAggregateRoot, TKey>();
		}

		/// <summary>
		///     Creates a <see cref="ISpecification{T}" /> in the form of <c>x => x.ID == id</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected ISpecification<TAggregateRoot> CreatePrimaryKeySpecification(TKey id)
		{
			Expression<Func<TAggregateRoot, bool>> predicate = this.CreatePrimaryKeyPredicate(id);
			ISpecification<TAggregateRoot> specification = new Specification<TAggregateRoot>(predicate);
			return specification;
		}
	}
}
