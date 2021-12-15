namespace Fluxera.Repository.UnitTests
{
	using System;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Logging.Configuration;
	using Moq;

	public static class MockLoggerExtensions
	{
		public static ILoggingBuilder AddMock(this ILoggingBuilder builder, Mock<ILogger> loggerMock)
		{
			builder.AddConfiguration();

			ILoggerProvider provider = new MockLoggerProvider(loggerMock);
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(provider));

			return builder;
		}

		public static Mock<ILogger<T>> VerifyTraceWasCalled<T>(this Mock<ILogger<T>> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Trace);
		}

		public static Mock<ILogger<T>> VerifyDebugWasCalled<T>(this Mock<ILogger<T>> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Debug);
		}

		public static Mock<ILogger<T>> VerifyInformationWasCalled<T>(this Mock<ILogger<T>> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Information);
		}

		public static Mock<ILogger<T>> VerifyWarningWasCalled<T>(this Mock<ILogger<T>> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Warning);
		}

		public static Mock<ILogger<T>> VerifyErrorWasCalled<T>(this Mock<ILogger<T>> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Error);
		}

		public static Mock<ILogger<T>> VerifyCriticalWasCalled<T>(this Mock<ILogger<T>> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Critical);
		}

		public static Mock<ILogger> VerifyTraceWasCalled(this Mock<ILogger> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Trace);
		}

		public static Mock<ILogger> VerifyDebugWasCalled(this Mock<ILogger> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Debug);
		}

		public static Mock<ILogger> VerifyInformationWasCalled(this Mock<ILogger> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Information);
		}

		public static Mock<ILogger> VerifyWarningWasCalled(this Mock<ILogger> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Warning);
		}

		public static Mock<ILogger> VerifyErrorWasCalled(this Mock<ILogger> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Error);
		}

		public static Mock<ILogger> VerifyCriticalWasCalled(this Mock<ILogger> logger)
		{
			return logger.VerifyLogWasCalled(LogLevel.Critical);
		}

		private static Mock<ILogger<T>> VerifyLogWasCalled<T>(this Mock<ILogger<T>> logger, LogLevel logLevel)
		{
			logger.Verify(
				x => x.Log(
					It.Is<LogLevel>(l => l == logLevel),
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => true),
					It.IsAny<Exception>(),
					It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

			return logger;
		}

		private static Mock<ILogger> VerifyLogWasCalled(this Mock<ILogger> logger, LogLevel logLevel)
		{
			logger.Verify(
				x => x.Log(
					It.Is<LogLevel>(l => l == logLevel),
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => true),
					It.IsAny<Exception>(),
					It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

			return logger;
		}
	}
}
