namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;

	public class TestCachingStrategyFactory : ICachingStrategyFactory
	{
		private object strategy;

		/// <inheritdoc />
		public ICachingStrategy<TAggregateRoot, TKey> CreateStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			this.strategy = new TestCachingStrategy<TAggregateRoot, TKey>();
			return (ICachingStrategy<TAggregateRoot, TKey>)this.strategy;
		}

		public TestCachingStrategy<TAggregateRoot, TKey> GetStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			return (TestCachingStrategy<TAggregateRoot, TKey>)this.strategy;
		}
	}
}
