namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for the caching feature builder.
	/// </summary>
	[PublicAPI]
	public interface ICachingOptionsBuilder
	{
		/// <summary>
		///     Use no caching strategy as default for all aggregates.
		/// </summary>
		IEntityCachingOptionsBuilder UseNoCaching();

		/// <summary>
		///     Use the standard caching strategy as default for all aggregates.
		/// </summary>
		IEntityCachingOptionsBuilder UseStandard();

		/// <summary>
		///     Use timeout caching strategy as default for all aggregates.
		/// </summary>
		/// <param name="expiration">The expiration time for the cache.</param>
		IEntityCachingOptionsBuilder UseTimeout(TimeSpan expiration);
	}
}
