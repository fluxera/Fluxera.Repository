namespace Sample.EntityFrameworkCore
{
	using Fluxera.Repository.EntityFrameworkCore;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Sample.Domain.Company;

	[PublicAPI]
	public sealed class SampleContext : RepositoryDbContextBase
	{
		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Filename=test.db");
			}
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Company>(entity =>
			{
				entity.ToTable("Companies");
			});
		}
	}
}
