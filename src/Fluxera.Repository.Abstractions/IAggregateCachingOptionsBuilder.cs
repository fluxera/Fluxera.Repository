namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for the aggregate-based caching overrides.
	/// </summary>
	[PublicAPI]
	public interface IAggregateCachingOptionsBuilder
	{
		/// <summary>
		///     Use no caching strategy as default for this aggregate.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
		IAggregateCachingOptionsBuilder UseNoCachingFor<TAggregateRoot>();

		/// <summary>
		///     Use no caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="type"></param>
		IAggregateCachingOptionsBuilder UseNoCachingFor(Type type);

		/// <summary>
		///     Use the standard caching strategy as default for this aggregate.
		/// </summary>
		/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
		IAggregateCachingOptionsBuilder UseStandardFor<TAggregateRoot>();

		/// <summary>
		///     Use the standard caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="type"></param>
		IAggregateCachingOptionsBuilder UseStandardFor(Type type);

		/// <summary>
		///     Use timeout caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="expiration">The expiration time for the cache.</param>
		/// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
		IAggregateCachingOptionsBuilder UseTimeoutFor<TAggregateRoot>(TimeSpan expiration);

		/// <summary>
		///     Use timeout caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="expiration">The expiration time for the cache.</param>
		IAggregateCachingOptionsBuilder UseTimeoutFor(Type type, TimeSpan expiration);
	}
}
