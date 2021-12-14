namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class RepositoryNameProvider : IRepositoryNameProvider
	{
		private readonly IRepositoryRegistry repositoryRegistry;

		public RepositoryNameProvider(IRepositoryRegistry repositoryRegistry)
		{
			this.repositoryRegistry = repositoryRegistry;
		}

		/// <inheritdoc />
		public RepositoryName? GetRepositoryNameFor(Type aggregateRootType)
		{
			return this.repositoryRegistry.GetRepositoryNameFor(aggregateRootType);
		}

		public RepositoryName? GetRepositoryNameFor<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			return this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
		}
	}
}
