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

	public class NoopTestRepository<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		public async Task AddAsync(TEntity item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
		{
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}

		/// <inheritdoc />
		public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions,
			CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TEntity> FindOneAsync(ISpecification<TEntity> specification, IQueryOptions<TEntity> queryOptions = null,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(ISpecification<TEntity> specification, Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
		{
			return false;
		}

		/// <inheritdoc />
		public virtual async Task<IReadOnlyCollection<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public virtual async Task<IReadOnlyCollection<TEntity>> FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions = null,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public bool IsDisposed => false;

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
		}

		/// <inheritdoc />
		public RepositoryName RepositoryName => new RepositoryName("Default");
	}
}
