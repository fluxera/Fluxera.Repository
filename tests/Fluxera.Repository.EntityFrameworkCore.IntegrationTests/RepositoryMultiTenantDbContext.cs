namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[PublicAPI]
	public sealed class RepositoryMultiTenantDbContext : EntityFrameworkCoreContext
	{
		private readonly TenantNameProvider tenantNameProvider;

		public RepositoryMultiTenantDbContext(TenantNameProvider tenantNameProvider = null)
		{
			this.tenantNameProvider = tenantNameProvider;
		}

		public DbSet<Person> People { get; set; }

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				string databaseName = "test";

				if(!string.IsNullOrWhiteSpace(this.tenantNameProvider?.Name))
				{
					databaseName += $"-{this.tenantNameProvider.Name}";
				}

				databaseName = $"{databaseName}.db";

				optionsBuilder.UseSqlite($"Filename={databaseName}");
			}
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>(entity =>
			{
				entity.ToTable("People");
				entity.OwnsOne(x => x.Address);
			});

			modelBuilder.UseRepositoryDefaults();
		}
	}
}
