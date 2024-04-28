namespace Fluxera.Repository.UnitTests.Core
{
	using System;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	public abstract class TestBase
	{
		protected static ServiceProvider BuildServiceProvider(Action<IServiceCollection> configure, Action<MediatRServiceConfiguration> configureMediatr = null)
		{
			IServiceCollection services = new ServiceCollection();

			services.AddLogging(builder =>
			{
				builder.SetMinimumLevel(LogLevel.Trace);
				builder.AddConsole();
			});

			configure.Invoke(services);

			MediatRServiceConfiguration configuration = new MediatRServiceConfiguration();
			configureMediatr?.Invoke(configuration);

			services.AddMediatR(configuration);

			return services.BuildServiceProvider();
		}
	}
}
