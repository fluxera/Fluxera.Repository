namespace Fluxera.Repository.Options
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;

	[PublicAPI]
	internal sealed class DomainEventsOptionsBuilder : IDomainEventsOptionsBuilder
	{
		public DomainEventsOptionsBuilder(RepositoryOptions repositoryOptions)
		{
			this.DomainEventsOptions = repositoryOptions.DomainEventsOptions;
		}

		DomainEventsOptions DomainEventsOptions { get; }

		public IDomainEventsOptionsBuilder AddEventHandlers(IEnumerable<Assembly> assemblies)
		{
			assemblies ??= Enumerable.Empty<Assembly>();

			foreach (Assembly assembly in assemblies)
			{
				this.AddEventHandlers(assembly);
			}

			return this;
		}

		public IDomainEventsOptionsBuilder AddEventHandlers(Assembly assembly)
		{
			this.DomainEventsOptions.DomainEventHandlersAssemblies.Add(assembly);

			return this;
		}
	}
}
