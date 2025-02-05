#pragma warning disable CS1998
namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;

	public class TestRepository<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => throw new NotImplementedException();

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public RepositoryName RepositoryName => throw new NotImplementedException();
	}
}
