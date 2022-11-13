namespace Sample.API
{
	using Fluxera.Repository;
	using Fluxera.Repository.EntityFrameworkCore;
	using Fluxera.Repository.LiteDB;
	using Fluxera.Repository.MongoDB;
	using Microsoft.Extensions.DependencyInjection;
	using Sample.Domain.Company;
	using Sample.EntityFrameworkCore;
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

					repositoryOptionsBuilder.EnableUnitOfWork();

					repositoryOptionsBuilder.AddSetting("Lite.Database", "sample.lite.db");
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

					repositoryOptionsBuilder.EnableUnitOfWork();

					repositoryOptionsBuilder.AddSetting("Mongo.ConnectionString", "mongodb://localhost:27017");
					repositoryOptionsBuilder.AddSetting("Mongo.Database", "sample");
				});
			});

			return services;
		}
	}
}
