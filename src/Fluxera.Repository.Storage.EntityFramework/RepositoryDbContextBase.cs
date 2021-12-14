namespace Fluxera.Repository.Storage.EntityFramework
{
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public abstract class RepositoryDbContextBase : DbContext
	{
		private readonly ILoggerFactory loggerFactory;
		private readonly IRepositoryRegistry repositoryRegistry;

		protected RepositoryDbContextBase(ILoggerFactory loggerFactory, IRepositoryRegistry repositoryRegistry)
		{
			this.loggerFactory = loggerFactory;
			this.repositoryRegistry = repositoryRegistry;
		}

		protected abstract RepositoryName RepositoryName { get; }

		/// <inheritdoc />
		protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(this.RepositoryName);

			EntityFrameworkPersistenceSettings settings = new EntityFrameworkPersistenceSettings
			{
				ConnectionString = (string)options.SettingsValues.GetOrDefault("EntityFramework.ConnectionString"),
				LogSQL = (bool)options.SettingsValues.GetOrDefault("EntityFramework.LogSQL"),
			};

			if(settings.LogSQL)
			{
				optionsBuilder.UseLoggerFactory(this.loggerFactory);
			}

			this.OnConfiguring(optionsBuilder, settings);
			base.OnConfiguring(optionsBuilder);
		}

		protected abstract void OnConfiguring(DbContextOptionsBuilder optionsBuilder, EntityFrameworkPersistenceSettings settings);
	}
}
