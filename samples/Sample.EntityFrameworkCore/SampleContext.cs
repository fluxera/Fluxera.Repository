namespace Sample.EntityFrameworkCore
{
	using Fluxera.Repository.EntityFrameworkCore;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class SampleContext : EntityFrameworkCoreContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(EntityFrameworkCoreContextOptions options)
		{
			options.UseDbContext<SampleDbContext>();
		}
	}
}
