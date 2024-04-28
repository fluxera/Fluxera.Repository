namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Repository.Options;

	public class TestRepositoryRegistry : IRepositoryRegistry
	{
		/// <inheritdoc />
		public RepositoryName GetRepositoryNameFor<TAggregateRoot>()
		{
			return new RepositoryName("Test");
		}

		/// <inheritdoc />
		public RepositoryName GetRepositoryNameFor(Type aggregateType)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public RepositoryOptions GetRepositoryOptionsFor(RepositoryName repositoryName)
		{
			return new RepositoryOptions(repositoryName)
			{
				InterceptionOptions =
				{
					IsEnabled = true
				},
				DomainEventsOptions =
				{
					IsEnabled = true,
				}
			};
		}

		/// <inheritdoc />
		public RepositoryOptions GetRepositoryOptionsFor(string repositoryName)
		{
			return this.GetRepositoryOptionsFor((RepositoryName)repositoryName);
		}
	}
}
