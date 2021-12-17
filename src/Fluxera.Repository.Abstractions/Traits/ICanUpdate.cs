namespace Fluxera.Repository.Traits
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanUpdate{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Update" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TAggregateRoot">Generic repository aggregate root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanUpdate<in TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <summary>
		///     Updates the given instance in the underlying store.
		/// </summary>
		/// <param name="item">The item to update.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken = default);

		/// <summary>
		///     Updates the given instances in the underlying store.
		/// </summary>
		/// <param name="items">The items to update.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default);
	}
}
