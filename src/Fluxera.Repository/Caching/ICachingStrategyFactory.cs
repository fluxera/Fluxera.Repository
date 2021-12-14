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
		/// <returns></returns>
		ICachingStrategy<TAggregateRoot> CreateStrategy<TAggregateRoot>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot>;
	}
}
