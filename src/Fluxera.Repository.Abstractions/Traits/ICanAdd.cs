namespace Fluxera.Repository.Traits
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanAdd{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Add" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TAggregateRoot">Generic repository entity root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanAdd<in TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <summary>
		///     Adds the given instance to the underlying store.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken = default);

		/// <summary>
		///     Adds the given instances to the underlying store.
		/// </summary>
		/// <param name="items">The items to add.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default);
	}
}
