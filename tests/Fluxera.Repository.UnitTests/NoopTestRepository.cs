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

	public class NoopTestRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		public async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
		{
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}

		/// <inheritdoc />
		public virtual async Task<TAggregateRoot> GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
		{
			return false;
		}

		/// <inheritdoc />
		public virtual async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
		{
			return default;
		}

		/// <inheritdoc />
		public bool IsDisposed { get; }

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
		}
	}
}
