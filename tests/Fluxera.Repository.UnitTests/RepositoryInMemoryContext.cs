namespace Fluxera.Repository.UnitTests
{
	using Fluxera.Repository.InMemory;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryInMemoryContext : InMemoryContext
	{
		/// <inheritdoc />
		public RepositoryInMemoryContext(string repositoryName, IRepositoryRegistry repositoryRegistry)
			: base(repositoryName, repositoryRegistry)
		{
		}
	}
}
