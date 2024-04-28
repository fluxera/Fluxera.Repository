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
		public IAggregateCachingOptionsBuilder UseNoCaching()
		{
			this.cachingOptions.DefaultStrategy = CachingStrategyNames.NoCaching;
			return new AggregateCachingOptionsBuilder(this.cachingOptions);
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseStandard()
		{
			this.cachingOptions.DefaultStrategy = CachingStrategyNames.Standard;
			return new AggregateCachingOptionsBuilder(this.cachingOptions);
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseTimeout(TimeSpan expiration)
		{
			this.cachingOptions.DefaultStrategy = CachingStrategyNames.Timeout;
			this.cachingOptions.DefaultExpiration = expiration;
			return new AggregateCachingOptionsBuilder(this.cachingOptions);
		}
	}
}
