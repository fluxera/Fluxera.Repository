namespace Fluxera.Repository.Caching
{
	using JetBrains.Annotations;

	/// <summary>
	///     Class holding constants that represent the available caching strategies.
	/// </summary>
	[PublicAPI]
	public static class CachingStrategyNames
	{
		/// <summary>
		///     No caching strategy.
		/// </summary>
		public const string NoCaching = "NoCaching";

		/// <summary>
		///     The standard caching strategy.
		/// </summary>
		public const string Standard = "Standard";

		/// <summary>
		///     The timeout caching strategy.
		/// </summary>
		public const string Timeout = "Timeout";
	}
}
