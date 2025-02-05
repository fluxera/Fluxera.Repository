namespace Fluxera.Repository.Options
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Caching;

	internal sealed class EntityCachingOptionsBuilder : IEntityCachingOptionsBuilder
	{
		private readonly CachingOptions cachingOptions;

		public EntityCachingOptionsBuilder(CachingOptions cachingOptions)
		{
			this.cachingOptions = cachingOptions;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseNoCachingFor<TEntity>()
		{
			Type type = typeof(TEntity);
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.NoCaching);
			return this;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseNoCachingFor(Type type)
		{
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.NoCaching);
			return this;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseStandardFor<TEntity>()
		{
			Type type = typeof(TEntity);
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Standard);
			return this;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseStandardFor(Type type)
		{
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Standard);
			return this;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseTimeoutFor<TEntity>(TimeSpan expiration)
		{
			Type type = typeof(TEntity);
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Timeout, expiration);
			return this;
		}

		/// <inheritdoc />
		public IEntityCachingOptionsBuilder UseTimeoutFor(Type type, TimeSpan expiration)
		{
			this.AddAggregateStrategyMapping(type, CachingStrategyNames.Timeout, expiration);
			return this;
		}

		private void AddAggregateStrategyMapping(Type type, string strategyName, TimeSpan? expiration = null)
		{
			Guard.Against.Null(type);
			Guard.Against.NullOrWhiteSpace(strategyName);
			Guard.Against.False(type.IsEntity(), message: $"The caching overrides can only use entity types: '{type.Name}'");

			if(!this.cachingOptions.AggregateStrategies.ContainsKey(type))
			{
				this.cachingOptions.AggregateStrategies.Add(type, new EntityCachingOverrideOptions(strategyName, expiration));
			}
			else
			{
				throw new InvalidOperationException(
					$"The entity type '{type.FullName}' was already used in the caching overrides.");
			}
		}
	}
}
