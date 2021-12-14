namespace Fluxera.Repository.Traits
{
	using System;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanGet{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Get" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TAggregateRoot">Generic repository entity root type.</typeparam>
	///// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanGet<TAggregateRoot /*, TKey*/> where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		/// <summary>
		///     Gets the instance from the underlying store for the given id.
		/// </summary>
		/// <param name="id">The id of the item to get.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item for the given id.</returns>
		Task<TAggregateRoot> GetAsync(string id, CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the value of the property specified by the selector.
		/// </summary>
		/// <param name="id">The id of the item to get.</param>
		/// <param name="selector">The result selector expression.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <typeparam name="TResult">The type of the result property.</typeparam>
		/// <returns>The result of the selector.</returns>
		Task<TResult> GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Checks if the item specified by the given id exist.
		/// </summary>
		/// <param name="id">The id of the item to check for.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><code>true</code> if the item exists; <code>false</code> otherwise.</returns>
		Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
	}
}
