namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IRepositoryNameProvider
	{
		RepositoryName? GetRepositoryNameFor(Type aggregateRootType);

		RepositoryName? GetRepositoryNameFor<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>;
	}
}
