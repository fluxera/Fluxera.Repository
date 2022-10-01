namespace Fluxera.Repository
{
	using System;
	using System.Diagnostics;
	using Microsoft.Extensions.Logging;

	internal static partial class LoggerExtensions
	{
		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Dispatching domain events (Before commit): Type = {Type}, Count = {Count}")]
		public static partial void LogDispatchingEventsBeforeCommit(this ILogger logger, string type, int count);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Dispatching domain events (After commit): Type = {Type}, Count = {Count}")]
		public static partial void LogDispatchingEventsAfterCommit(this ILogger logger, string type, int count);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Intercepting before {Operation}: {AggregateRoot}")]
		public static partial void LogInterceptingBeforeOperation(this ILogger logger, string operation, string aggregateRoot);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Critical, "A critical error occurred trying to perform {Operation}: {AggregateRoot}")]
		public static partial void LogErrorOccurredForOperation(this ILogger logger, string operation, string aggregateRoot, Exception exception);

		[DebuggerStepThrough]
		public static void LogCancellationMessage(this ILogger logger, string cancellationMessage)
		{
			if(logger.IsEnabled(LogLevel.Information))
			{
				logger.LogInformation(cancellationMessage);
			}
		}
	}
}
