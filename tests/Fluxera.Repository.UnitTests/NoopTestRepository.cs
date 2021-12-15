namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;

	public class NoopTestRepository<T> : IRepository<T> where T : AggregateRoot<T>
	{
		/// <inheritdoc />
		async Task ICanAdd<T>.AddAsync(T item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanAdd<T>.AddAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanUpdate<T>.UpdateAsync(T item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanUpdate<T>.UpdateAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(T item, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
		}

		/// <inheritdoc />
		async Task<T> ICanGet<T>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<T>.GetAsync<TResult>(string id, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<T>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		async Task<T> ICanFind<T>.FindOneAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<T>.FindOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<T>.ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return false;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<T>> ICanFind<T>.FindManyAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<T>.FindManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<T>.CountAsync(CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<T>.CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return default;
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<T>.IsDisposed { get; }
	}
}
