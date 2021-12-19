namespace Fluxera.Repository.Caching
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	/// <summary>
	///     The cache prefix manager service contract.
	/// </summary>
	[PublicAPI]
	public interface ICachePrefixManager
	{
		/// <summary>
		///     Gets the global cache counter value (generation).
		/// </summary>
		/// <returns></returns>
		Task<long> GetGlobalCounterAsync();

		/// <summary>
		///     Increments the global cache counter value (generation) and by that
		///     invalidating the whole cache for every entity.
		/// </summary>
		/// <returns></returns>
		Task IncrementGlobalCounterAsync();
	}
}
