namespace Fluxera.Repository.MongoDB
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The extensions methods for configuring a MongoDB repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		static RepositoryBuilderExtensions()
		{
#pragma warning disable CS0618
			BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618

			BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new DecimalSerializationProvider());
		}

		/// <summary>
		///     Add a MongoDB repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddMongoRepository<TContext>(this IRepositoryBuilder builder,
			Action<IRepositoryOptionsBuilder> configure)
			where TContext : MongoContext
		{
			return builder.AddMongoRepository(typeof(TContext), configure);
		}

		/// <summary>
		///     Add a MongoDB repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="dbContextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddMongoRepository(this IRepositoryBuilder builder,
			Type dbContextType, Action<IRepositoryOptionsBuilder> configure)
		{
			return builder.AddMongoRepository("Default", dbContextType, configure);
		}

		/// <summary>
		///     Adds a MongoDB repository for the given repository name. The repository options
		///     are configured using the options builder config action. Additional MongoDB related
		///     conventions can be registered using the configure convention action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddMongoRepository<TContext>(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
			where TContext : MongoContext
		{
			return builder.AddMongoRepository(repositoryName, typeof(TContext), configure);
		}

		/// <summary>
		///     Adds a MongoDB repository for the given repository name. The repository options
		///     are configured using the options builder config action. Additional MongoDB related
		///     conventions can be registered using the configure convention action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="contextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddMongoRepository(this IRepositoryBuilder builder,
			string repositoryName, Type contextType, Action<IRepositoryOptionsBuilder> configure)
		{
			ConventionPack pack = new ConventionPack();
			pack.UseRepositoryDefaults();

			ConventionRegistry.Register("ConventionPack", pack, _ => true);

			builder.Services.AddScoped(contextType);
			builder.Services.AddScoped<MongoContextProvider>();
			builder.Services.AddNamedTransient<IUnitOfWork>(serviceBuilder =>
			{
				serviceBuilder.AddNameFor<MongoUnitOfWork>(repositoryName);
			});

			return builder.AddRepository(repositoryName, typeof(MongoRepository<,>), x =>
			{
				configure.Invoke(x);

				x.AddSetting("Repository.ContextType", contextType);
			});
		}
	}
}
