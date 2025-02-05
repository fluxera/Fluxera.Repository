namespace Fluxera.Repository.Caching
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	internal sealed class CachingStrategyFactory : ICachingStrategyFactory
	{
		private readonly ICacheKeyProvider cacheKeyProvider;
		private readonly ICachingProviderFactory cachingProviderFactory;
		private readonly ILoggerFactory loggerFactory;
		private readonly IRepositoryRegistry repositoryRegistry;

		public CachingStrategyFactory(
			IRepositoryRegistry repositoryRegistry,
			ICachingProviderFactory cachingProviderFactory,
			ICacheKeyProvider cacheKeyProvider,
			ILoggerFactory loggerFactory)
		{
			this.repositoryRegistry = repositoryRegistry;
			this.cachingProviderFactory = cachingProviderFactory;
			this.cacheKeyProvider = cacheKeyProvider;
			this.loggerFactory = loggerFactory;
		}

		public ICachingStrategy<TEntity, TKey> CreateStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TEntity>();
			RepositoryOptions repositoryOptions = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			CachingOptions cachingOptions = repositoryOptions.CachingOptions;

			ICachingStrategy<TEntity, TKey> cachingStrategy;

			bool isEnabled = cachingOptions.IsEnabled;
			if(isEnabled)
			{
				ICachingProvider cachingProvider = this.cachingProviderFactory.CreateCachingProvider();

				string strategyName = cachingOptions.DefaultStrategy;
				TimeSpan? expiration = cachingOptions.DefaultExpiration;

				// Check for an aggregate override.
				if(cachingOptions.AggregateStrategies.ContainsKey(typeof(TEntity)))
				{
					EntityCachingOverrideOptions cachingOverrideOptions = cachingOptions.AggregateStrategies[typeof(TEntity)];
					strategyName = cachingOverrideOptions.StrategyName;
					expiration = cachingOverrideOptions.Expiration;
				}

				cachingStrategy = strategyName switch
				{
					CachingStrategyNames.NoCaching => new NoCachingStrategy<TEntity, TKey>(),
					CachingStrategyNames.Standard => new StandardCachingStrategy<TEntity, TKey>(repositoryName, cachingProvider, this.cacheKeyProvider, this.loggerFactory),
					CachingStrategyNames.Timeout => new TimeoutCachingStrategy<TEntity, TKey>(repositoryName, cachingProvider, this.cacheKeyProvider, this.loggerFactory, expiration),
					_ => new NoCachingStrategy<TEntity, TKey>()
				};
			}
			else
			{
				cachingStrategy = new NoCachingStrategy<TEntity, TKey>();
			}

			return cachingStrategy;
		}
	}
}
