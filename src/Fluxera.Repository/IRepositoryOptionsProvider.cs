namespace Fluxera.Repository
{
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IRepositoryOptionsProvider
	{
		RepositoryOptions GetRepositoryOptions(RepositoryName repositoryName);
	}
}
