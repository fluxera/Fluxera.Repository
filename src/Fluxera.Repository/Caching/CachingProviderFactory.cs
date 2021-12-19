namespace Fluxera.Repository.Caching
{
	using Fluxera.Extensions.Common;
	using Microsoft.Extensions.Caching.Distributed;
	using Microsoft.Extensions.Logging;

	internal sealed class CachingProviderFactory : ICachingProviderFactory
	{
		private readonly IDistributedCache distributedCache;
		private readonly IHashCalculator hashCalculator;
		private readonly ILoggerFactory loggerFactory;

		public CachingProviderFactory(
			IDistributedCache distributedCache,
			ILoggerFactory loggerFactory,
			IHashCalculator hashCalculator)
		{
			this.distributedCache = distributedCache;
			this.loggerFactory = loggerFactory;
			this.hashCalculator = hashCalculator;
		}

		/// <inheritdoc />
		public ICachingProvider CreateCachingProvider()
		{
			return new CachingProvider(this.loggerFactory, this.hashCalculator, this.distributedCache);
		}
	}
}
