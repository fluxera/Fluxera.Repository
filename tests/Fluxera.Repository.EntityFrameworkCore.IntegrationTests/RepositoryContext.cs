namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryContext : EntityFrameworkCoreContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(EntityFrameworkCoreContextOptions options)
		{
			options.UseDbContext<RepositoryDbContext>();
		}
	}
}
