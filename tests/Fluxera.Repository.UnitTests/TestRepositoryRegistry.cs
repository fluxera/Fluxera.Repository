namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Repository.Options;

	public class TestRepositoryRegistry : IRepositoryRegistry
	{
		/// <inheritdoc />
		public RepositoryName GetRepositoryNameFor<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public RepositoryName GetRepositoryNameFor(Type repositoryType)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public RepositoryOptions GetRepositoryOptionsFor(RepositoryName repositoryName)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public IReadOnlyCollection<RepositoryOptions> GetRepositoryOptions()
		{
			throw new NotImplementedException();
		}
	}
}
