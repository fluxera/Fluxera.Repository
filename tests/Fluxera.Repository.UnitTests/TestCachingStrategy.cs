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
		public bool AddMultipleWasCalled;
		public bool AddSingleWasCalled;
		public bool CountWasCalled;
		public bool CountWithPredicateWasCalled;
		public bool ExistsWasCalled;
		public bool ExistsWithPredicateWasCalled;
		public bool FindManyWithPredicateAndSelectorWasCalled;
		public bool FindManyWithPredicateWasCalled;
		public bool FindOneWithPredicateAndSelectorWasCalled;
		public bool FindOneWithPredicateWasCalled;
		public bool GetWasCalled;
		public bool GetWithSelectorWasCalled;
		public bool RemoveMultipleWasCalled;
		public bool RemoveSingleWasCalled;
		public bool UpdateMultipleWasCalled;
		public bool UpdateSingleWasCalled;

		/// <inheritdoc />
		async Task ICachingStrategy<T>.AddAsync(T item)
		{
			this.AddSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.AddAsync(IEnumerable<T> items)
		{
			this.AddMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.UpdateAsync(T item)
		{
			this.UpdateSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.UpdateAsync(IEnumerable<T> items)
		{
			this.UpdateMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.RemoveAsync(string id)
		{
			this.RemoveSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<T>.RemoveAsync(IEnumerable<string> ids)
		{
			this.RemoveMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task<T> ICachingStrategy<T>.GetAsync(string id, Func<Task<T>> setter)
		{
			this.GetWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<T>.GetAsync<TResult>(string id, Expression<Func<T, TResult>> selector, Func<Task<TResult>> setter)
		{
			this.GetWithSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<T>.CountAsync(Func<Task<long>> setter)
		{
			this.CountWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<T>.CountAsync(Expression<Func<T, bool>> predicate, Func<Task<long>> setter)
		{
			this.CountWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<T> ICachingStrategy<T>.FindOneAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, Func<Task<T>> setter)
		{
			this.FindOneWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<T>.FindOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, Func<Task<TResult>> setter)
		{
			this.FindOneWithPredicateAndSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<T>> ICachingStrategy<T>.FindManyAsync(Expression<Func<T, bool>> predicate, IQueryOptions<T>? queryOptions, Func<Task<IReadOnlyCollection<T>>> setter)
		{
			this.FindManyWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICachingStrategy<T>.FindManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T>? queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter)
		{
			this.FindManyWithPredicateAndSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<T>.ExistsAsync(string id, Func<Task<bool>> setter)
		{
			this.ExistsWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<T>.ExistsAsync(Expression<Func<T, bool>> predicate, Func<Task<bool>> setter)
		{
			this.ExistsWithPredicateWasCalled = true;

			return default;
		}
	}
}
