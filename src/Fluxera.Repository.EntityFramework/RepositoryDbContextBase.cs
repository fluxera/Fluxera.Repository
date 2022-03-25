namespace Fluxera.Repository.EntityFramework
{
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A base class for implementing <see cref="DbContext" /> classes to use with the repository.
	/// </summary>
	[PublicAPI]
	public abstract class RepositoryDbContextBase : DbContext
	{
		private readonly ILoggerFactory loggerFactory;
		private readonly IRepositoryRegistry repositoryRegistry;

		/// <summary>
		///     Creates a new instance of the <see cref="RepositoryDbContextBase" /> type.
		/// </summary>
		/// <param name="loggerFactory"></param>
		/// <param name="repositoryRegistry"></param>
		protected RepositoryDbContextBase(ILoggerFactory loggerFactory, IRepositoryRegistry repositoryRegistry)
		{
			this.loggerFactory = loggerFactory;
			this.repositoryRegistry = repositoryRegistry;
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		protected abstract string RepositoryName { get; }

		/// <inheritdoc />
		protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor((RepositoryName)this.RepositoryName);

			EntityFrameworkPersistenceSettings settings = new EntityFrameworkPersistenceSettings
			{
				ConnectionString = (string)options.SettingsValues.GetOrDefault("EntityFramework.ConnectionString")!,
				LogSQL = (bool)(options.SettingsValues.GetOrDefault("EntityFramework.LogSQL") ?? false)
			};

			if(settings.LogSQL)
			{
				optionsBuilder.UseLoggerFactory(this.loggerFactory);
			}

			this.OnConfiguring(optionsBuilder, settings);

			base.OnConfiguring(optionsBuilder);
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		///     Configure the given <see cref="DbContextOptionsBuilder" /> using the given settings.
		/// </summary>
		protected abstract void OnConfiguring(DbContextOptionsBuilder optionsBuilder, EntityFrameworkPersistenceSettings settings);
	}
}
