namespace Fluxera.Repository.UnitTests
{
	using System;
	using Microsoft.Extensions.DependencyInjection;

	public class TestBase
	{
		protected static IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure)
		{
			IServiceCollection services = new ServiceCollection();

			configure(services);

			return services.BuildServiceProvider();
		}
	}
}
