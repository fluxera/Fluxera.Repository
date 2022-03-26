namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	public sealed class RepositoryDbContext : RepositoryDbContextBase
	{
		/// <inheritdoc />
		public RepositoryDbContext(ILoggerFactory loggerFactory, IRepositoryRegistry repositoryRegistry)
			: base(loggerFactory, repositoryRegistry)
		{
		}

		/// <inheritdoc />
		protected override string RepositoryName => "RepositoryUnderTest";

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder, EntityFrameworkPersistenceSettings settings)
		{
			// https://entityframeworkcore.com/providers-inmemory
			optionsBuilder.UseSqlite(settings.ConnectionString);
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>(entity =>
			{
				entity.HasKey(p => p.ID);
			});
		}
	}
}
