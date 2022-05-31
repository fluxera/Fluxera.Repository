namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Query;

	public class TestCachingStrategy<TAggregateRoot, TKey> : ICachingStrategy<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
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
		async Task ICachingStrategy<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item)
		{
			this.AddSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items)
		{
			this.AddMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item)
		{
			this.UpdateSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items)
		{
			this.UpdateMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TAggregateRoot, TKey>.RemoveAsync(TKey id)
		{
			this.RemoveSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TKey> ids)
		{
			this.RemoveMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICachingStrategy<TAggregateRoot, TKey>.GetAsync(TKey id, Func<Task<TAggregateRoot>> setter)
		{
			this.GetWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, Func<Task<TResult>> setter)
		{
			this.GetWithSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<TAggregateRoot, TKey>.CountAsync(Func<Task<long>> setter)
		{
			this.CountWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, Func<Task<long>> setter)
		{
			this.CountWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICachingStrategy<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, Func<Task<TAggregateRoot>> setter)
		{
			this.FindOneWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, Func<Task<TResult>> setter)
		{
			this.FindOneWithPredicateAndSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICachingStrategy<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, Func<Task<IReadOnlyCollection<TAggregateRoot>>> setter)
		{
			this.FindManyWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICachingStrategy<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter)
		{
			this.FindManyWithPredicateAndSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<TAggregateRoot, TKey>.ExistsAsync(TKey id, Func<Task<bool>> setter)
		{
			this.ExistsWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, Func<Task<bool>> setter)
		{
			this.ExistsWithPredicateWasCalled = true;

			return default;
		}
	}
}
