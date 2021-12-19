namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IInterceptionOptionsBuilder
	{
		IInterceptionOptionsBuilder AddInterceptors(IEnumerable<Assembly> assemblies);

		IInterceptionOptionsBuilder AddInterceptors(Assembly assembly);
	}
}
