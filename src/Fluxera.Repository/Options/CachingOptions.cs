namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Repository.Caching;
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the caching options.
	/// </summary>
	[PublicAPI]
	public sealed class CachingOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="CachingOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public CachingOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
			this.DefaultStrategy = CachingStrategyNames.Standard;
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Flag, if the caching is enabled.
		/// </summary>
		public bool IsEnabled { get; internal set; }

		/// <summary>
		///     The name of the default strategy to use.
		/// </summary>
		internal string DefaultStrategy { get; set; }

		/// <summary>
		///     The default expiration time for the timeout strategy.
		/// </summary>
		internal TimeSpan? DefaultExpiration { get; set; }

		/// <summary>
		///     Gets the mapping from an aggregate to a caching strategy name.
		/// </summary>
		internal IDictionary<Type, AggregateCachingOverrideOptions> AggregateStrategies { get; } = new Dictionary<Type, AggregateCachingOverrideOptions>();
	}
}
