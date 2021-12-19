namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryOptions
	{
		public RepositoryOptions(RepositoryName repositoryName, Type repositoryType)
		{
			this.RepositoryName = repositoryName;
			this.RepositoryType = repositoryType;
			this.ValidationOptions = new ValidationOptions(repositoryName);
			this.DomainEventsOptions = new DomainEventsOptions(repositoryName);
			this.CachingOptions = new CachingOptions(repositoryName);
			this.InterceptionOptions = new InterceptionOptions(repositoryName);
		}

		public RepositoryName RepositoryName { get; }

		public Type RepositoryType { get; }

		public IList<Type> AggregateRootTypes { get; } = new List<Type>();

		public IDictionary<string, object> SettingsValues { get; } = new Dictionary<string, object>();

		public ValidationOptions ValidationOptions { get; }

		public DomainEventsOptions DomainEventsOptions { get; set; }

		public CachingOptions CachingOptions { get; set; }

		public InterceptionOptions InterceptionOptions { get; set; }
	}
}
