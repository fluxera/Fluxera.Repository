namespace Fluxera.Repository.Options
{
	using System;

	internal readonly struct EntityCachingOverrideOptions
	{
		public EntityCachingOverrideOptions(string strategyName, TimeSpan? expiration = null)
		{
			this.StrategyName = strategyName;
			this.Expiration = expiration;
		}

		internal string StrategyName { get; }

		internal TimeSpan? Expiration { get; }
	}
}
