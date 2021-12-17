namespace Fluxera.Repository.UnitTests
{
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;

	public class TestCachingStrategyFactory : ICachingStrategyFactory
	{
		private object strategy;

		/// <inheritdoc />
		public ICachingStrategy<TAggregateRoot, TKey> CreateStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			this.strategy = new TestCachingStrategy<TAggregateRoot, TKey>();
			return (ICachingStrategy<TAggregateRoot, TKey>)this.strategy;
		}

		public TestCachingStrategy<TAggregateRoot, TKey> GetStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			return (TestCachingStrategy<TAggregateRoot, TKey>)this.strategy;
		}
	}
}
