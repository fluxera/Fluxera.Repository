namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;

	public class NoopTestRepository<T> : IRepository<T> where T : AggregateRoot<T>
	{
		/// <inheritdoc />
		public async Task AddAsync(T item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task AddAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateAsync(T item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task UpdateAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(T item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(string id, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public async Task RemoveAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}

		/// <inheritdoc />
		public virtual async Task<T> GetAsync(string id, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> GetAsync<TResult>(string id, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		public virtual async Task<IReadOnlyCollection<T>> FindManyAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
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
