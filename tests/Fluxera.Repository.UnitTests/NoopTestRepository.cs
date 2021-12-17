namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;

	public class NoopTestRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <inheritdoc />
		public async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
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
		public async Task RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
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
		public async Task<TAggregateRoot> FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public virtual async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
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
		public bool IsDisposed { get; }

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
		}
	}
}
