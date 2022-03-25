namespace Fluxera.Repository.Options
{
	using System;

	internal struct AggregateCachingOverrideOptions
	{
		public AggregateCachingOverrideOptions(string strategyName, TimeSpan? expiration = null)
		{
			this.StrategyName = strategyName;
			this.Expiration = expiration;
		}

		internal string StrategyName { get; }

		internal TimeSpan? Expiration { get; }
	}
}
