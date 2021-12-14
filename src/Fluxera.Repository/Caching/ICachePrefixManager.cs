namespace Fluxera.Repository.Caching
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface ICachePrefixManager
	{
		Task<long> GetGlobalCounterAsync();

		Task IncrementGlobalCounterAsync();
	}
}
