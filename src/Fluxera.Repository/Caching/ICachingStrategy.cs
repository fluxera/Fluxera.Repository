﻿namespace Fluxera.Repository.Caching
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for caching strategies.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public interface ICachingStrategy<TEntity, in TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     The strategy method is called when a new item is added.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task AddAsync(TEntity item);

		/// <summary>
		///     The strategy method is called when new items are added.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		Task AddAsync(IEnumerable<TEntity> items);

		/// <summary>
		///     The strategy method is called when a item is updated.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task UpdateAsync(TEntity item);

		/// <summary>
		///     The strategy method is called when items are updated.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		Task UpdateAsync(IEnumerable<TEntity> items);

		/// <summary>
		///     The strategy method is called when an item is removed by ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task RemoveAsync(TKey id);

		/// <summary>
		///     The strategy method is called when items are removed by ID.
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		Task RemoveAsync(IEnumerable<TKey> ids);

		/// <summary>
		///     The strategy method is called when an item is retrieved by ID.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TEntity> GetAsync(TKey id, Func<Task<TEntity>> setter);

		/// <summary>
		///     The strategy method is called when an item is retrieved by ID with a selector expression.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="id"></param>
		/// <param name="selector"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TResult> GetAsync<TResult>(TKey id,
			Expression<Func<TEntity, TResult>> selector,
			Func<Task<TResult>> setter);

		/// <summary>
		///     The strategy method is called when a count is executed.
		/// </summary>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<long> CountAsync(Func<Task<long>> setter);

		/// <summary>
		///     The strategy method is called when a count with a predicate is executed.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<long> CountAsync(
			Expression<Func<TEntity, bool>> predicate,
			Func<Task<long>> setter);

		/// <summary>
		///     The strategy method is called when a sum is executed.
		/// </summary>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TResult> SumAsync<TResult>(Func<Task<TResult>> setter)
			where TResult : IComparable, IConvertible, IFormattable, IComparable<TResult>, IEquatable<TResult>;

		/// <summary>
		///     The strategy method is called when a count with a predicate is executed.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TResult> SumAsync<TResult>(
			Expression<Func<TEntity, bool>> predicate,
			Func<Task<TResult>> setter)
			where TResult : IComparable, IConvertible, IFormattable, IComparable<TResult>, IEquatable<TResult>;

		/// <summary>
		///     The strategy method is called when an average is executed.
		/// </summary>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TResult> AverageAsync<TResult>(Func<Task<TResult>> setter)
			where TResult : IComparable, IConvertible, IFormattable, IComparable<TResult>, IEquatable<TResult>;

		/// <summary>
		///     The strategy method is called when an average with a predicate is executed.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TResult> AverageAsync<TResult>(
			Expression<Func<TEntity, bool>> predicate,
			Func<Task<TResult>> setter)
			where TResult : IComparable, IConvertible, IFormattable, IComparable<TResult>, IEquatable<TResult>;

		/// <summary>
		///     The strategy method is called when a find-one with a predicate is executed.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="queryOptions"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TEntity> FindOneAsync(
			Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions,
			Func<Task<TEntity>> setter);

		/// <summary>
		///     The strategy method is called when a find-one with a predicate and selector expression is executed.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<TResult> FindOneAsync<TResult>(
			Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions,
			Func<Task<TResult>> setter);

		/// <summary>
		///     The strategy method is called when a find-many with a predicate is executed.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="queryOptions"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<IReadOnlyCollection<TEntity>> FindManyAsync(
			Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions,
			Func<Task<IReadOnlyCollection<TEntity>>> setter);

		/// <summary>
		///     The strategy method is called when a find-many with a predicate and selector is executed.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="queryOptions"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(
			Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector,
			IQueryOptions<TEntity> queryOptions,
			Func<Task<IReadOnlyCollection<TResult>>> setter);

		/// <summary>
		///     The strategy method is called when an exists is executed with an ID.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<bool> ExistsAsync(TKey id, Func<Task<bool>> setter);

		/// <summary>
		///     The strategy method is called when an exists is executed with a predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="setter">A setter function that gets called when no item was found in the cache.</param>
		/// <returns></returns>
		Task<bool> ExistsAsync(
			Expression<Func<TEntity, bool>> predicate,
			Func<Task<bool>> setter);
	}
}
