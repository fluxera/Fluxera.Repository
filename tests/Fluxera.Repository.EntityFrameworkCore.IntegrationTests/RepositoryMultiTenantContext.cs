namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryMultiTenantContext : EntityFrameworkCoreContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(EntityFrameworkCoreContextOptions options)
		{
			options.UseDbContext<RepositoryMultiTenantDbContext>();
		}
	}
}
