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

		public ICachingStrategy<TAggregateRoot, TKey> CreateStrategy<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions repositoryOptions = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			CachingOptions cachingOptions = repositoryOptions.CachingOptions;

			ICachingStrategy<TAggregateRoot, TKey> cachingStrategy;

			bool isEnabled = cachingOptions.IsEnabled;
			if(isEnabled)
			{
				ICachingProvider cachingProvider = this.cachingProviderFactory.CreateCachingProvider();

				string strategyName = cachingOptions.DefaultStrategy;
				TimeSpan? expiration = cachingOptions.DefaultExpiration;

				// Check for an aggregate override.
				if(cachingOptions.AggregateStrategies.ContainsKey(typeof(TAggregateRoot)))
				{
					AggregateCachingOverrideOptions cachingOverrideOptions = cachingOptions.AggregateStrategies[typeof(TAggregateRoot)];
					strategyName = cachingOverrideOptions.StrategyName;
					expiration = cachingOverrideOptions.Expiration;
				}

				cachingStrategy = strategyName switch
				{
					CachingStrategyNames.NoCaching => new NoCachingStrategy<TAggregateRoot, TKey>(),
					CachingStrategyNames.Standard => new StandardCachingStrategy<TAggregateRoot, TKey>(repositoryName, cachingProvider, this.cacheKeyProvider, this.loggerFactory),
					CachingStrategyNames.Timeout => new TimeoutCachingStrategy<TAggregateRoot, TKey>(repositoryName, cachingProvider, this.cacheKeyProvider, this.loggerFactory, expiration),
					_ => new NoCachingStrategy<TAggregateRoot, TKey>()
				};
			}
			else
			{
				cachingStrategy = new NoCachingStrategy<TAggregateRoot, TKey>();
			}

			return cachingStrategy;
		}
	}
}
