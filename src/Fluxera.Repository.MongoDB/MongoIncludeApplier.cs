namespace Fluxera.Repository.MongoDB
{
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	internal sealed class MongoIncludeApplier : NoopIncludeApplier
	{
		/// <inheritdoc />
		public MongoIncludeApplier(ILoggerFactory loggerFactory)
			: base(loggerFactory, "MongoDB")
		{
		}
	}
}
