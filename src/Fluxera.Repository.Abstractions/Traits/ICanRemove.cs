namespace Fluxera.Repository.Traits
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanRemove{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Delete" methods of the repository.
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </summary>
	/// <typeparam name="TAggregateRoot">Generic repository aggregate root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanRemove<TAggregateRoot, in TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Deletes the given instance from the underlying store.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken = default);

		/// <summary>
		///     Deletes the item with the given id from the underlying store.
		/// </summary>
		/// <param name="id">The id of the item to remove.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task RemoveAsync(TKey id, CancellationToken cancellationToken = default);

		/// <summary>
		///     Deletes all items that match the given expression from the underlying store.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);

		/// <summary>
		///     Deletes all items that match the given specification from the underlying store.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default);

		/// <summary>
		///     Removes the given instances from the underlying store.
		/// </summary>
		/// <param name="items">The items to remove.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default);
	}
}
