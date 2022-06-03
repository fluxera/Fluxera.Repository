namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[PublicAPI]
	public sealed class RepositoryDbContext : RepositoryDbContextBase
	{
		/// <inheritdoc />
		protected override string RepositoryName => "RepositoryUnderTest";

		public DbSet<Person> People { get; set; }

		public DbSet<Company> Companies { get; set; }

		public DbSet<Employee> Employees { get; set; }

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				// https://entityframeworkcore.com/providers-inmemory
				optionsBuilder.UseSqlite("Filename=test.db");
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

			modelBuilder.Entity<Employee>(entity =>
			{
				entity
					.Property(e => e.SalaryDecimal)
					.HasConversion<double>();
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
