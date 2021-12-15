namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Query;

	public class TestCachingStrategy<T> : ICachingStrategy<T> where T : AggregateRoot<T>
	{
		/// <inheritdoc />
		async Task ICachingStrategy<T>.AddAsync(T item)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.AddAsync(IEnumerable<T> items)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.UpdateAsync(T item)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.UpdateAsync(IEnumerable<T> items)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.DeleteAsync(string id)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.DeleteAsync(IEnumerable<string> ids)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<T> ICachingStrategy<T>.GetAsync(string id, Func<Task<T>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<T>.GetAsync<TResult>(string id, Expression<Func<T, TResult>> selector, Func<Task<TResult>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<T>.CountAsync(Func<Task<long>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<T>.CountAsync(Expression<Func<T, bool>> predicate, Func<Task<long>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<T> ICachingStrategy<T>.FindOneAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, Func<Task<T>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<T>.FindOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, Func<Task<TResult>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<T>> ICachingStrategy<T>.FindManyAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, Func<Task<IReadOnlyCollection<T>>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICachingStrategy<T>.FindManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<T>.ExistsAsync(string id, Func<Task<bool>> setter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<T>.ExistsAsync(Expression<Func<T, bool>> predicate, Func<Task<bool>> setter)
		{
			throw new NotImplementedException();
		}
	}
}
