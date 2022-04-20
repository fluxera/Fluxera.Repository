namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Concurrent;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[UsedImplicitly]
	internal sealed class DbContextFactory : IDbContextFactory
	{
		private readonly ConcurrentDictionary<RepositoryName, Type> dbContextMap = new ConcurrentDictionary<RepositoryName, Type>();

		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IServiceProvider serviceProvider;

		public DbContextFactory(IServiceProvider serviceProvider, IRepositoryRegistry repositoryRegistry)
		{
			this.serviceProvider = serviceProvider;
			this.repositoryRegistry = repositoryRegistry;
		}

		public DbContext CreateDbContext<TAggregateRoot, TKey>() where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();

			DbContext dbContext = this.dbContextMap.TryGetValue(repositoryName, out Type dbContextType)
				? this.CreateContext(dbContextType)
				: this.RegisterDbContext(repositoryName);

			return dbContext;
		}

		private DbContext RegisterDbContext(RepositoryName repositoryName)
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			Type dbContextType = options.Settings.GetOrDefault("EntityFrameworkCore.DbContext") as Type;

			Guard.Against.Null(dbContextType, nameof(dbContextType));

			if(!this.dbContextMap.TryAdd(repositoryName, dbContextType!))
			{
				throw new InvalidOperationException($"Could not add DbContext type for repository '{repositoryName}'.");
			}

			return this.CreateContext(dbContextType!);
		}

		private DbContext CreateContext(Type dbContextType)
		{
			DbContext dbContext = this.serviceProvider.GetService(dbContextType) as DbContext ?? Activator.CreateInstance(dbContextType) as DbContext;

			Guard.Against.Null(dbContext, nameof(dbContext));

			return dbContext!;
		}
	}
}
