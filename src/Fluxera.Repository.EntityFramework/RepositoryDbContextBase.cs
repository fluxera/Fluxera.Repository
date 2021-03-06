namespace Fluxera.Repository.EntityFrameworkCore
{
	using Fluxera.Enumeration.EntityFrameworkCore;
	using Fluxera.StronglyTypedId.EntityFrameworkCore;
	using Fluxera.ValueObject.EntityFrameworkCore;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	///     A base class for implementing <see cref="DbContext" /> classes to use with the repository.
	/// </summary>
	[PublicAPI]
	public abstract class RepositoryDbContextBase : DbContext
	{
		/// <summary>
		///     Gets the repository name.
		/// </summary>
		protected abstract string RepositoryName { get; }

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseLazyLoadingProxies()
				.UseStronglyTypedId();

			base.OnConfiguring(optionsBuilder);
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.UseSpatial();
			//modelBuilder.UseTemporal();
			modelBuilder.UseEnumeration();
			modelBuilder.UsePrimitiveValueObject();
			modelBuilder.UseStronglyTypedId();
			modelBuilder.UseStronglyTypedIdValueGenerator();
			modelBuilder.UseSequentialGuidStringIdValueGenerator();
			modelBuilder.UseReferences();

			base.OnModelCreating(modelBuilder);
		}
	}
}
