namespace Fluxera.Repository.Caching
{
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for factories that created instances of <see cref="ICachingStrategy{TAggregateRoot,TKey}" />.
	/// </summary>
	[PublicAPI]
	public interface ICachingStrategyFactory
	{
		/// <summary>
		///     Creates the cache strategy to use for the repository and <see cref="TAggregateRoot" />.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <typeparam name="TKey">The type if the keys.</typeparam>
		/// <returns></returns>
		ICachingStrategy<TAggregateRoot, TKey> CreateStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;
	}
}
