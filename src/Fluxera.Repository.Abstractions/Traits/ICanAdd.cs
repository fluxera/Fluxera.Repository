namespace Fluxera.Repository.Traits
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanAdd{TEntity,TKey}" /> interface
	///     exposes only the "Add" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TEntity">Generic repository entity root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanAdd<in TEntity, in TKey> : IProvideRepositoryName<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Adds the given instance to the underlying store.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task AddAsync(TEntity item, CancellationToken cancellationToken = default);

		/// <summary>
		///     Adds the given instances to the underlying store.
		/// </summary>
		/// <param name="items">The items to add.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);
	}
}
