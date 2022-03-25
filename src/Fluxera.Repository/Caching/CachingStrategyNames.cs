namespace Fluxera.Repository.Caching
{
	using JetBrains.Annotations;

	[PublicAPI]
	public static class CachingStrategyNames
	{
		public const string NoCaching = "NoCaching";
		public const string Standard = "Standard";
		public const string Timeout = "Timeout";
	}
}
