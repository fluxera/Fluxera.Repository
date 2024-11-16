namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Repository.UnitTests.Core.ReferenceAggregate;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Diagnostics;

	[PublicAPI]
	public sealed class RepositoryDbContext : DbContext
	{
		private readonly string databaseName;

		public RepositoryDbContext()
		{
		}

		internal RepositoryDbContext(string databaseName)
		{
			this.databaseName = databaseName;
		}

		public DbSet<Person> People { get; set; }

		public DbSet<Company> Companies { get; set; }

		public DbSet<Employee> Employees { get; set; }

		public DbSet<Reference> References { get; set; }

		public DbSet<Invoice> Invoices { get; set; }

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(this.databaseName is null
					? GlobalFixture.ConnectionString
					: GlobalFixture.ConnectionString.Replace($"Database={GlobalFixture.Database}", $"Database={this.databaseName}"));

				optionsBuilder.ConfigureWarnings(builder =>
				{
					builder.Ignore(RelationalEventId.PendingModelChangesWarning);
				});
			}
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>(entity =>
			{
				entity.ToTable("People");
				entity.ComplexProperty(x => x.Address).IsRequired();
				//entity.OwnsOne(x => x.Address);

				entity.UseRepositoryDefaults();
			});


			modelBuilder.Entity<Company>(entity =>
			{
				entity.UseRepositoryDefaults();
			});

			modelBuilder.Entity<Employee>(entity =>
			{
				entity
					.Property(x => x.SalaryDecimal)
					.HasConversion<double>();

				entity
					.Property(x => x.SalaryNullableDecimal)
					.HasConversion<double?>();

				entity.UseRepositoryDefaults();
			});

			modelBuilder.Entity<Reference>(entity =>
			{
				entity.UseRepositoryDefaults();
			});

			modelBuilder.Entity<Invoice>(entity =>
			{
				entity.UseRepositoryDefaults();
			});

			modelBuilder.Entity<InvoiceItem>(entity =>
			{
				entity.HasKey(x => x.ID);

				entity
					.Property(x => x.Amount)
					.HasConversion<double>();

				entity.UseRepositoryDefaults();
			});
		}
	}
}
