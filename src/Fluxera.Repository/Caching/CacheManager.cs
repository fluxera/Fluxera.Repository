namespace Fluxera.Repository.Caching
{
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     A helper to access the <see cref="ICachePrefixManager" /> statically.
	/// </summary>
	[PublicAPI]
	public static class CacheManager
	{
		/// <summary>
		///     Gets or sets the <see cref="ICachePrefixManager" />.
		/// </summary>
		public static ICachePrefixManager CachePrefixManager { get; set; }

		/// <summary>
		///     Gets the global cache counter value (generation).
		/// </summary>
		/// <returns></returns>
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

			await CachePrefixManager!.IncrementGlobalCounterAsync().ConfigureAwait(false);
		}
	}
}
