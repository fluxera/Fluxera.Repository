namespace Fluxera.Repository.EntityFramework
{
	using System;
	using System.Collections.Concurrent;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	internal sealed class DbContextFactory : IDbContextFactory
	{
		private readonly ConcurrentDictionary<RepositoryName, Type> dbContextMap = new ConcurrentDictionary<RepositoryName, Type>();

		private readonly ILoggerFactory loggerFactory;
		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IServiceProvider serviceProvider;

		public DbContextFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IRepositoryRegistry repositoryRegistry)
		{
			this.serviceProvider = serviceProvider;
			this.loggerFactory = loggerFactory;
			this.repositoryRegistry = repositoryRegistry;
		}

		public DbContext CreateDbContext<TAggregateRoot, TKey>() where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();

			DbContext dbContext = this.dbContextMap.TryGetValue(repositoryName, out Type? dbContextType)
				? this.CreateContext(dbContextType)
				: this.RegisterDbContext(repositoryName);

			return dbContext;
		}

		private DbContext RegisterDbContext(RepositoryName repositoryName)
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			Type? dbContextType = options.SettingsValues.GetOrDefault("EntityFramework.DbContext") as Type;

			Guard.Against.Null(dbContextType, nameof(dbContextType));

			if(!this.dbContextMap.TryAdd(repositoryName, dbContextType!))
			{
				throw new InvalidOperationException($"Could not add DbContext type for repository '{repositoryName}'.");
			}

			return this.CreateContext(dbContextType!);
		}

		private DbContext CreateContext(Type dbContextType)
		{
			DbContext? dbContext = this.serviceProvider.GetService(dbContextType) as DbContext;
			if(dbContext is null)
			{
				dbContext = Activator.CreateInstance(dbContextType, new object[]
				{
					this.loggerFactory,
					this.repositoryRegistry
				}) as DbContext;
			}

			Guard.Against.Null(dbContext, nameof(dbContext));

			return dbContext!;
		}
	}
}
