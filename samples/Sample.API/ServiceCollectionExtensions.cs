namespace Sample.API
{
	using Fluxera.Repository;
	using Fluxera.Repository.EntityFrameworkCore;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.LiteDB;
	using Fluxera.Repository.MongoDB;
	using global::LiteDB;
	using Microsoft.Extensions.DependencyInjection;
	using Sample.Domain.Company;
	using Sample.Domain.Company.Handlers;
	using Sample.EntityFrameworkCore;
	using Sample.InMemory;
	using Sample.LiteDB;
	using Sample.MongoDB;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services)
		{
			services.AddDbContext<SampleDbContext>();

			services.AddRepository(repositoryBuilder =>
			{
				repositoryBuilder.AddEntityFrameworkRepository<SampleContext>(repositoryOptionsBuilder =>
				{
					repositoryOptionsBuilder.UseFor<Company>();

					repositoryOptionsBuilder.AddDomainEventHandling(domainEventsOptionsBuilder =>
					{
						domainEventsOptionsBuilder.AddDomainEventHandler<CompanyAddedHandler>();

						domainEventsOptionsBuilder.AddCrudDomainEventsFactory<SampleCrudDomainEventsFactory>();

						domainEventsOptionsBuilder.EnableAutomaticCrudDomainEvents();
					});

					//repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			return services;
		}

		public static IServiceCollection AddLiteDB(this IServiceCollection services)
		{
			services.AddRepository(repositoryBuilder =>
			{
				repositoryBuilder.AddLiteRepository<SampleLiteContext>(repositoryOptionsBuilder =>
				{
					repositoryOptionsBuilder.UseFor<Company>();

					repositoryOptionsBuilder.AddDomainEventHandling(domainEventsOptionsBuilder =>
					{
						domainEventsOptionsBuilder.AddDomainEventHandler<CompanyAddedHandler>();

						domainEventsOptionsBuilder.AddCrudDomainEventsFactory<SampleCrudDomainEventsFactory>();

						domainEventsOptionsBuilder.EnableAutomaticCrudDomainEvents();
					});

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			// TODO: Try to generalize this. We have the used entities in the settings.
			BsonMapper.Global.Entity<Company>()
				.Id(x => x.ID)
				.Ignore(x => x.DomainEvents);

			return services;
		}

		public static IServiceCollection AddMongoDB(this IServiceCollection services)
		{
			services.AddRepository(repositoryBuilder =>
			{
				repositoryBuilder.AddMongoRepository<SampleMongoContext>(repositoryOptionsBuilder =>
				{
					repositoryOptionsBuilder.UseFor<Company>();

					repositoryOptionsBuilder.AddDomainEventHandling(domainEventsOptionsBuilder =>
					{
						domainEventsOptionsBuilder.AddDomainEventHandler<CompanyAddedHandler>();

						domainEventsOptionsBuilder.AddCrudDomainEventsFactory<SampleCrudDomainEventsFactory>();

						domainEventsOptionsBuilder.EnableAutomaticCrudDomainEvents();
					});

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			return services;
		}

		public static IServiceCollection AddInMemory(this IServiceCollection services)
		{
			services.AddRepository(repositoryBuilder =>
			{
				repositoryBuilder.AddInMemoryRepository<SampleInMemoryContext>(repositoryOptionsBuilder =>
				{
					repositoryOptionsBuilder.UseFor<Company>();

					repositoryOptionsBuilder.AddDomainEventHandling(domainEventsOptionsBuilder =>
					{
						domainEventsOptionsBuilder.AddDomainEventHandler<CompanyAddedHandler>();

						domainEventsOptionsBuilder.AddCrudDomainEventsFactory<SampleCrudDomainEventsFactory>();

						domainEventsOptionsBuilder.EnableAutomaticCrudDomainEvents();
					});

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			return services;
		}
	}
}
