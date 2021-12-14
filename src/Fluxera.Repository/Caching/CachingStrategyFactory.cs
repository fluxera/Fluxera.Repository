namespace Fluxera.Repository.Caching
{
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

		public ICachingStrategy<TAggregateRoot> CreateStrategy<TAggregateRoot>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions repositoryOptions = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			bool isEnabled = repositoryOptions.CachingOptions.Enabled;
			if(isEnabled)
			{
				ICachingProvider cachingProvider = this.cachingProviderFactory.CreateCachingProvider();
				return new StandardCachingStrategy<TAggregateRoot>(repositoryName, cachingProvider,
					this.cacheKeyProvider, this.loggerFactory);
			}

			return new NoCachingStrategy<TAggregateRoot>();
		}
	}
}
