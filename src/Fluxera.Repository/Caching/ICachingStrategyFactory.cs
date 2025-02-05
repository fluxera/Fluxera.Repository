namespace Fluxera.Repository.Caching
{
	using System;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for factories that created instances of <see cref="ICachingStrategy{TAggregateRoot,TKey}" />.
	/// </summary>
	[PublicAPI]
	public interface ICachingStrategyFactory
	{
		/// <summary>
		///     Creates the cache strategy to use for the repository and aggregate root.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TKey">The type if the keys.</typeparam>
		/// <returns></returns>
		ICachingStrategy<TEntity, TKey> CreateStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>;
	}
}
