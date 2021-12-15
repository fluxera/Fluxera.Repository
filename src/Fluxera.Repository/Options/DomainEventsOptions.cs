namespace Fluxera.Repository.Options
{
	using System.Collections.Generic;
	using System.Reflection;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class DomainEventsOptions
	{
		public DomainEventsOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		public RepositoryName RepositoryName { get; }

		public bool IsEnabled { get; set; }

		public IList<Assembly> DomainEventHandlersAssemblies { get; } = new List<Assembly>();
	}
}
