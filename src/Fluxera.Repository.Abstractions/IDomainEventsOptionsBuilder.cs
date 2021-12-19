namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Reflection;

	public interface IDomainEventsOptionsBuilder
	{
		IDomainEventsOptionsBuilder AddEventHandlers(IEnumerable<Assembly>? assemblies);

		IDomainEventsOptionsBuilder AddEventHandlers(Assembly assembly);
	}
}
