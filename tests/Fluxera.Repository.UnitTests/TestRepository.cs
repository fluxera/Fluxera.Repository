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

	public class TestRepository<T> : IRepository<T> where T : AggregateRoot<T>
	{
		/// <inheritdoc />
		async Task ICanAdd<T>.AddAsync(T item, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanAdd<T>.AddAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanUpdate<T>.UpdateAsync(T item, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanUpdate<T>.UpdateAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(T item, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICanRemove<T>.RemoveAsync(IEnumerable<T> items, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<T> ICanGet<T>.GetAsync(string id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<T>.GetAsync<TResult>(string id, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<T>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<T> ICanFind<T>.FindOneAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<T>.FindOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<T>.ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<T>> ICanFind<T>.FindManyAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<T>.FindManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<T>.CountAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<T>.CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<T>.IsDisposed { get; }
	}
}
