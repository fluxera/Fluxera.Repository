namespace Fluxera.Repository.EntityFrameworkCore
{
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	///     A base class for implementing <see cref="DbContext" /> classes to use with the repository.
	/// </summary>
	[PublicAPI]
	public abstract class RepositoryDbContextBase : DbContext
	{
		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseRepositoryDefaults();

			base.OnConfiguring(optionsBuilder);
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.UseRepositoryDefaults();

			base.OnModelCreating(modelBuilder);
		}
	}
}
