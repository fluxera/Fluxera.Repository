namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	public abstract class TestBase
	{
		protected static IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure)
		{
			IServiceCollection services = new ServiceCollection();

			services.AddLogging(builder =>
			{
				builder.SetMinimumLevel(LogLevel.Trace);
				builder.AddConsole();
			});

			configure(services);

			return services.BuildServiceProvider();
		}
	}
}
