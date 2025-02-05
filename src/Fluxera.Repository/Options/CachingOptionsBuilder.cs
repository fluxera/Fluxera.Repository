namespace Fluxera.Repository.Options
{
	using System;
	using Fluxera.Repository.Caching;

	internal sealed class CachingOptionsBuilder : ICachingOptionsBuilder
	{
		private readonly CachingOptions cachingOptions;

		public CachingOptionsBuilder(RepositoryOptions repositoryOptions)
		{
			this.cachingOptions = repositoryOptions.CachingOptions;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseNoCaching()
		{
			this.cachingOptions.DefaultStrategy = CachingStrategyNames.NoCaching;
			return new EntityCachingOptionsBuilder(this.cachingOptions);
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseStandard()
		{
			this.cachingOptions.DefaultStrategy = CachingStrategyNames.Standard;
			return new EntityCachingOptionsBuilder(this.cachingOptions);
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseTimeout(TimeSpan expiration)
		{
			this.cachingOptions.DefaultStrategy = CachingStrategyNames.Timeout;
			this.cachingOptions.DefaultExpiration = expiration;
			return new EntityCachingOptionsBuilder(this.cachingOptions);
		}
	}
}
