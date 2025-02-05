namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for the entity-based caching overrides.
	/// </summary>
	[PublicAPI]
	public interface IEntityCachingOptionsBuilder
	{
		/// <summary>
		///     Use no caching strategy as default for this aggregate.
		/// </summary>
		/// <typeparam name="TEntity">The aggregate root type.</typeparam>
		IEntityCachingOptionsBuilder UseNoCachingFor<TEntity>();

		/// <summary>
		///     Use no caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="type"></param>
		IEntityCachingOptionsBuilder UseNoCachingFor(Type type);

		/// <summary>
		///     Use the standard caching strategy as default for this aggregate.
		/// </summary>
		/// <typeparam name="TEntity">The aggregate root type.</typeparam>
		IEntityCachingOptionsBuilder UseStandardFor<TEntity>();

		/// <summary>
		///     Use the standard caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="type"></param>
		IEntityCachingOptionsBuilder UseStandardFor(Type type);

		/// <summary>
		///     Use timeout caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="expiration">The expiration time for the cache.</param>
		/// <typeparam name="TEntity">The aggregate root type.</typeparam>
		IEntityCachingOptionsBuilder UseTimeoutFor<TEntity>(TimeSpan expiration);

		/// <summary>
		///     Use timeout caching strategy as default for this aggregate.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="expiration">The expiration time for the cache.</param>
		IEntityCachingOptionsBuilder UseTimeoutFor(Type type, TimeSpan expiration);
	}
}
