namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Caching;

	public class TestCachingStrategyFactory : ICachingStrategyFactory
	{
		private object strategy;

		/// <inheritdoc />
		public ICachingStrategy<TEntity, TKey> CreateStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			this.strategy = new TestCachingStrategy<TEntity, TKey>();
			return (ICachingStrategy<TEntity, TKey>)this.strategy;
		}

		public TestCachingStrategy<TEntity, TKey> GetStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			return (TestCachingStrategy<TEntity, TKey>)this.strategy;
		}
	}
}
