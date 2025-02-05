namespace Fluxera.Repository.Traits
{
	using System;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanGet{TEntity,TKey}" /> interface
	///     exposes only the "Get" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TEntity">Generic repository entity root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanGet<TEntity, in TKey> : IProvideRepositoryName<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Gets the instance from the underlying store for the given id.
		/// </summary>
		/// <param name="id">The id of the item to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item for the given id.</returns>
		Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the value of the property specified by the selector.
		/// </summary>
		/// <param name="id">The id of the item to get.</param>
		/// <param name="selector">The result selector expression.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <typeparam name="TResult">The type of the result property.</typeparam>
		/// <returns>The result of the selector.</returns>
		Task<TResult> GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the count of existing items of the underlying store.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item count.</returns>
		Task<long> CountAsync(CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the count of items of the underlying store that match the given predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item count.</returns>
		Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the count of items of the underlying store that match the given specification.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item count.</returns>
		Task<long> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

		/// <summary>
		///     Checks if the item specified by the given id exist.
		/// </summary>
		/// <param name="id">The id of the item to check for.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><code>true</code> if the item exists; <code>false</code> otherwise.</returns>
		Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);
	}
}
