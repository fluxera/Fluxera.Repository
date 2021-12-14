namespace Fluxera.Repository.Caching
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class CachePrefixManager : ICachePrefixManager
	{
		private readonly ICachingProvider cachingProvider;

		public CachePrefixManager(ICachingProviderFactory cachingProviderFactory)
		{
			this.cachingProvider = cachingProviderFactory.CreateCachingProvider();
		}

		public async Task<long> GetGlobalCounterAsync()
		{
			return await this.cachingProvider
				.GetAsync<long>("Repositories/Counter")
				.ConfigureAwait(false);
		}

		public async Task IncrementGlobalCounterAsync()
		{
			await this.cachingProvider
				.IncrementAsync("Repositories/Counter", 1)
				.ConfigureAwait(false);
		}
	}
}
