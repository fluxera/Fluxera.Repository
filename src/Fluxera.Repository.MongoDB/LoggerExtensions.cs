namespace Fluxera.Repository.MongoDB
{
	using System.Diagnostics;
	using Microsoft.Extensions.Logging;

	internal static partial class LoggerExtensions
	{
		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Warning,
			"The UnitOfWork is enabled for the repository {RepositoryName}, but the underlying cluster is no replica set. No transactions are started/managed.")]
		public static partial void LogUnitOfWorkEnabledWithoutReplicaSet(this ILogger logger, RepositoryName repositoryName);
	}
}
