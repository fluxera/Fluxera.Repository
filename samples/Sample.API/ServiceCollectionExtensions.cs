namespace Sample.API
{
	using Fluxera.Repository;
	using Fluxera.Repository.EntityFrameworkCore;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.LiteDB;
	using Fluxera.Repository.MongoDB;
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
				repositoryBuilder.AddEntityFrameworkRepository<SampleDbContext>(repositoryOptionsBuilder =>
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
					});

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

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
					});

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			return services;
		}
	}
}
