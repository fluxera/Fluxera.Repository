namespace Fluxera.Repository.UnitTests
{
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;

	public class TestCachingStrategyFactory : ICachingStrategyFactory
	{
		/// <inheritdoc />
		public ICachingStrategy<TAggregateRoot> CreateStrategy<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			return new TestCachingStrategy<TAggregateRoot>();
		}
	}
}
