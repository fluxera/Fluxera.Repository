namespace Fluxera.Repository.UnitTests
{
	using Microsoft.Extensions.Logging;
	using Moq;

	public class MockLoggerProvider : ILoggerProvider
	{
		private readonly Mock<ILogger> loggerMock;

		public MockLoggerProvider(Mock<ILogger> loggerMock)
		{
			this.loggerMock = loggerMock;
		}


		/// <inheritdoc />
		public ILogger CreateLogger(string categoryName)
		{
			return this.loggerMock.Object;
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}
	}
}
