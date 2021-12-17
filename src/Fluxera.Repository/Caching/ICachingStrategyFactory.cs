namespace Fluxera.Repository.Caching
{
	using Fluxera.Entity;
	using JetBrains.Annotations;

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
