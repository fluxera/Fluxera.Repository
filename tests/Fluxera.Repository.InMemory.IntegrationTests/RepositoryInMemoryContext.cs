namespace Fluxera.Repository.InMemory.IntegrationTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryInMemoryContext : InMemoryContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(InMemoryContextOptions options)
		{
		}
	}
}
