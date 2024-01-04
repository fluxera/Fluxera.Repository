namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[PublicAPI]
	public sealed class RepositoryMultiTenantDbContext : DbContext
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
				string databaseName = GlobalFixture.Database;

				if(!string.IsNullOrWhiteSpace(this.tenantNameProvider?.Name))
				{
					databaseName += $"-{this.tenantNameProvider.Name}";
				}

				databaseName = $"{databaseName}";

				optionsBuilder.UseSqlServer(GlobalFixture.ConnectionString.Replace($"Database={GlobalFixture.Database}", $"Database={databaseName}"));
			}
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>(entity =>
			{
				entity.ToTable("People");
//#if NET8_0_OR_GREATER
//				entity.ComplexProperty(x => x.Address);
//#endif
				entity.OwnsOne(x => x.Address);

				entity.UseRepositoryDefaults();
			});
		}
	}
}
