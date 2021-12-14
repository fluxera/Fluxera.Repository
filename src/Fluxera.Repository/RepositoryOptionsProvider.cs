namespace Fluxera.Repository
{
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class RepositoryOptionsProvider : IRepositoryOptionsProvider
	{
		private readonly IRepositoryRegistry repositoryRegistry;

		public RepositoryOptionsProvider(IRepositoryRegistry repositoryRegistry)
		{
			this.repositoryRegistry = repositoryRegistry;
		}

		public RepositoryOptions GetRepositoryOptions(RepositoryName repositoryName)
		{
			return this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
		}
	}
}
