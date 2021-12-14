namespace Fluxera.Repository.Caching
{
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class CacheManager
	{
		public static ICachePrefixManager? CachePrefixManager { get; set; }

		public static async Task<long> GetGlobalCounterAsync()
		{
			long counter = CachePrefixManager != null
				? await CachePrefixManager.GetGlobalCounterAsync().ConfigureAwait(false)
				: 0;
			return counter;
		}

		/// <summary>
		///     Increment the global counter to invalidate the complete cache for every repository and aggregate.
		/// </summary>
		/// <returns></returns>
		public static async Task InvalidateGlobal()
		{
			Guard.Against.Null(CachePrefixManager, nameof(CachePrefixManager),
				"Configure the CacheManager.CachePrefixManager in order to handle invalidating the global cache.");

			await CachePrefixManager.IncrementGlobalCounterAsync().ConfigureAwait(false);
		}
	}
}
