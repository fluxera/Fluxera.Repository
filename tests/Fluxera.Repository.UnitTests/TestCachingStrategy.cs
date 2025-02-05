#pragma warning disable CS1998
namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Query;

	public class TestCachingStrategy<TEntity, TKey> : ICachingStrategy<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		public bool AddMultipleWasCalled;
		public bool AddSingleWasCalled;
		public bool AverageWasCalled;
		public bool AverageWithPredicateWasCalled;
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
		public bool SumWasCalled;
		public bool SumWithPredicateWasCalled;
		public bool UpdateMultipleWasCalled;
		public bool UpdateSingleWasCalled;

		/// <inheritdoc />
		async Task ICachingStrategy<TEntity, TKey>.AddAsync(TEntity item)
		{
			this.AddSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TEntity, TKey>.AddAsync(IEnumerable<TEntity> items)
		{
			this.AddMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TEntity, TKey>.UpdateAsync(TEntity item)
		{
			this.UpdateSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TEntity, TKey>.UpdateAsync(IEnumerable<TEntity> items)
		{
			this.UpdateMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TEntity, TKey>.RemoveAsync(TKey id)
		{
			this.RemoveSingleWasCalled = true;
		}

		/// <inheritdoc />
		async Task ICachingStrategy<TEntity, TKey>.RemoveAsync(IEnumerable<TKey> ids)
		{
			this.RemoveMultipleWasCalled = true;
		}

		/// <inheritdoc />
		async Task<TEntity> ICachingStrategy<TEntity, TKey>.GetAsync(TKey id, Func<Task<TEntity>> setter)
		{
			this.GetWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector, Func<Task<TResult>> setter)
		{
			this.GetWithSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<TEntity, TKey>.CountAsync(Func<Task<long>> setter)
		{
			this.CountWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<long> ICachingStrategy<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, Func<Task<long>> setter)
		{
			this.CountWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TEntity, TKey>.SumAsync<TResult>(Func<Task<TResult>> setter)
		{
			this.SumWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TEntity, TKey>.SumAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Func<Task<TResult>> setter)
		{
			this.SumWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TEntity, TKey>.AverageAsync<TResult>(Func<Task<TResult>> setter)
		{
			this.AverageWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TEntity, TKey>.AverageAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Func<Task<TResult>> setter)
		{
			this.AverageWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TEntity> ICachingStrategy<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions, Func<Task<TEntity>> setter)
		{
			this.FindOneWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<TResult> ICachingStrategy<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, Func<Task<TResult>> setter)
		{
			this.FindOneWithPredicateAndSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICachingStrategy<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate, IQueryOptions<TEntity> queryOptions, Func<Task<IReadOnlyCollection<TEntity>>> setter)
		{
			this.FindManyWithPredicateWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICachingStrategy<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter)
		{
			this.FindManyWithPredicateAndSelectorWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<TEntity, TKey>.ExistsAsync(TKey id, Func<Task<bool>> setter)
		{
			this.ExistsWasCalled = true;

			return default;
		}

		/// <inheritdoc />
		async Task<bool> ICachingStrategy<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, Func<Task<bool>> setter)
		{
			this.ExistsWithPredicateWasCalled = true;

			return default;
		}
	}
}
