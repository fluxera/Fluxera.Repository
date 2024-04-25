namespace Fluxera.Repository.Extensions
{
	using System;
	using System.Diagnostics;
	using Microsoft.Extensions.Logging;

	internal static partial class LoggerExtensions
	{
		[DebuggerStepThrough]
		[LoggerMessage(1000, LogLevel.Debug, "Dispatched domain events: Type = {Type}, Count = {Count}")]
		public static partial void LogDispatchedDomainEvents(this ILogger logger, string type, int count);

		[DebuggerStepThrough]
		[LoggerMessage(1001, LogLevel.Debug, "Intercepting before {Operation}: {AggregateRoot}")]
		public static partial void LogInterceptingBeforeOperation(this ILogger logger, string operation, string aggregateRoot);

		[DebuggerStepThrough]
		[LoggerMessage(1002, LogLevel.Warning, "The storage implementation '{StorageName}' doesn't support the Include query option.")]
		public static partial void LogStorageNotSupportsIncludeQueryOption(this ILogger logger, string storageName);

		[DebuggerStepThrough]
		[LoggerMessage(1003, LogLevel.Critical, "A critical error occurred trying to perform {Operation}: {AggregateRoot}")]
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
