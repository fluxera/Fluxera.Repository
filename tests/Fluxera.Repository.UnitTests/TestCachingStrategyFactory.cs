namespace Fluxera.Repository.UnitTests
{
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;

	public class TestCachingStrategyFactory : ICachingStrategyFactory
	{
		private object strategy;

		/// <inheritdoc />
		public ICachingStrategy<TAggregateRoot> CreateStrategy<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			this.strategy = new TestCachingStrategy<TAggregateRoot>();
			return (ICachingStrategy<TAggregateRoot>)this.strategy;
		}

		public TestCachingStrategy<T> GetStrategy<T>() where T : AggregateRoot<T>
		{
			return (TestCachingStrategy<T>)this.strategy;
		}
	}
}
