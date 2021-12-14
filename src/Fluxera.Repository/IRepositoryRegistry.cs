namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IRepositoryRegistry
	{
		RepositoryName GetRepositoryNameFor<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>;

		RepositoryName GetRepositoryNameFor(Type repositoryType);

		RepositoryOptions GetRepositoryOptionsFor(RepositoryName repositoryName);

		IReadOnlyCollection<RepositoryOptions> GetRepositoryOptions();

		internal void Register(RepositoryName repositoryName, RepositoryOptions repositoryOptions);
	}
}
