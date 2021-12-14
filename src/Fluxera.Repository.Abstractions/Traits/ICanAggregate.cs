namespace Fluxera.Repository.Traits
{
	using System;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanAggregate{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Aggregate" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TAggregateRoot">Generic repository aggregate root type.</typeparam>
	/////  <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanAggregate<TAggregateRoot /*, TKey*/> where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
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
		Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);
	}
}
