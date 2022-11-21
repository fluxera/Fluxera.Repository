namespace Fluxera.Repository.Traits
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanFind{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Find" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TAggregateRoot">Generic repository aggregate root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanFind<TAggregateRoot, in TKey> : IProvideRepositoryName<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Finds the first item that matches the predicate expression.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The result item, or <c>null</c> if no item was found.</returns>
		Task<TAggregateRoot> FindOneAsync(
			Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds the first item that matches the specification expression.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The result item, or <c>null</c> if no item was found.</returns>
		Task<TAggregateRoot> FindOneAsync(
			ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds the value of the property specified by the selector of the first item that matches the predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="selector">The result selector expression.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <typeparam name="TResult">The type of the result property.</typeparam>
		/// <returns>The result of the selector.</returns>
		Task<TResult> FindOneAsync<TResult>(
			Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds the value of the property specified by the selector of the first item that matches the specification.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="selector">The result selector expression.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <typeparam name="TResult">The type of the result property.</typeparam>
		/// <returns>The result of the selector.</returns>
		Task<TResult> FindOneAsync<TResult>(
			ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Checks if there is an item that matches the given predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><code>true</code> if an item exists; <code>false</code> otherwise.</returns>
		Task<bool> ExistsAsync(
			Expression<Func<TAggregateRoot, bool>> predicate,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Checks if there is an item that matches the given specification.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><code>true</code> if an item exists; <code>false</code> otherwise.</returns>
		Task<bool> ExistsAsync(
			ISpecification<TAggregateRoot> specification,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds items matching the predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The result items, or an empty enumerable if no item was found.</returns>
		Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(
			Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds items matching the specification.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The result items, or an empty enumerable if no item was found.</returns>
		Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(
			ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds the values of the property specified by the selector of items matching the predicate.
		/// </summary>
		/// <param name="predicate">The specification to match.</param>
		/// <param name="selector">The result selector expression.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <typeparam name="TResult">The type of the result property.</typeparam>
		/// <returns>The results of the selector.</returns>
		Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(
			Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Finds the values of the property specified by the selector of items matching the specification.
		/// </summary>
		/// <param name="specification">The predicate to match.</param>
		/// <param name="selector">The result selector expression.</param>
		/// <param name="queryOptions">The options for paging and sorting.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <typeparam name="TResult">The type of the result property.</typeparam>
		/// <returns>The results of the selector.</returns>
		Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(
			ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default);
	}
}
