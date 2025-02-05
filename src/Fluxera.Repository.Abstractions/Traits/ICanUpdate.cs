namespace Fluxera.Repository.Traits
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanUpdate{TEntity,TKey}" /> interface
	///     exposes only the "Update" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TEntity">Generic repository aggregate root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanUpdate<in TEntity, in TKey> : IProvideRepositoryName<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Updates the given instance in the underlying store.
		/// </summary>
		/// <param name="item">The item to update.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task UpdateAsync(TEntity item, CancellationToken cancellationToken = default);

		/// <summary>
		///     Updates the given instances in the underlying store.
		/// </summary>
		/// <param name="items">The items to update.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);
	}
}
