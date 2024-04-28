namespace Fluxera.Repository.Options
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;

	internal sealed class AggregateCachingOptionsBuilder : IAggregateCachingOptionsBuilder
	{
		private readonly CachingOptions cachingOptions;

		public AggregateCachingOptionsBuilder(CachingOptions cachingOptions)
		{
			this.cachingOptions = cachingOptions;
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseNoCachingFor<TAggregateRoot>()
		{
			Type type = typeof(TAggregateRoot);
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.NoCaching);
			return this;
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseNoCachingFor(Type type)
		{
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.NoCaching);
			return this;
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseStandardFor<TAggregateRoot>()
		{
			Type type = typeof(TAggregateRoot);
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Standard);
			return this;
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseStandardFor(Type type)
		{
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Standard);
			return this;
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseTimeoutFor<TAggregateRoot>(TimeSpan expiration)
		{
			Type type = typeof(TAggregateRoot);
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Timeout, expiration);
			return this;
		}

		/// <inheritdoc />
		public IAggregateCachingOptionsBuilder UseTimeoutFor(Type type, TimeSpan expiration)
		{
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Timeout, expiration);
			return this;
		}

		private void AddAggregateStrategyMapping(Type type, string strategyName, TimeSpan? expiration = null)
		{
			Guard.Against.Null(type, nameof(type));
			Guard.Against.NullOrWhiteSpace(strategyName, nameof(strategyName));
			Guard.Against.False(type.IsAggregateRoot(), nameof(type), $"The caching overrides can only use aggregate root types: '{type.Name}'");

			if(!this.cachingOptions.AggregateStrategies.ContainsKey(type))
			{
				this.cachingOptions.AggregateStrategies.Add(type, new AggregateCachingOverrideOptions(strategyName, expiration));
			}
			else
			{
				throw new InvalidOperationException(
					$"The aggregate root type '{type.FullName}' was already used in the caching overrides.");
			}
		}
	}
}
