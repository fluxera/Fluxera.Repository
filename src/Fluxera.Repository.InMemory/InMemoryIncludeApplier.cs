namespace Fluxera.Repository.InMemory
{
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	internal sealed class InMemoryIncludeApplier : NoopIncludeApplier
	{
		/// <inheritdoc />
		public InMemoryIncludeApplier(ILoggerFactory loggerFactory)
			: base(loggerFactory, "InMemory")
		{
		}
	}
}
