namespace Fluxera.Repository.LiteDB
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using global::LiteDB;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The extensions methods to configure a LiteDb repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Add a LiteDB repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddLiteRepository<TContext>(this IRepositoryBuilder builder,
			Action<IRepositoryOptionsBuilder> configure)
			where TContext : LiteContext
		{
			return builder.AddLiteRepository(typeof(TContext), configure);
		}

		/// <summary>
		///     Add a LiteDB repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="dbContextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddLiteRepository(this IRepositoryBuilder builder,
			Type dbContextType, Action<IRepositoryOptionsBuilder> configure)
		{
			return builder.AddLiteRepository("Default", dbContextType, configure);
		}

		/// <summary>
		///     Adds a LiteDB repository for the given repository name. The repository options
		///     are configured using the options builder configure action. Additional LiteDB
		///     related mappings can be configures using the mapper configuration action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddLiteRepository<TContext>(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
			where TContext : LiteContext
		{
			return builder.AddLiteRepository(repositoryName, typeof(TContext), configure);
		}

		/// <summary>
		///     Adds a LiteDB repository for the given repository name. The repository options
		///     are configured using the options builder configure action. Additional LiteDB
		///     related mappings can be configures using the mapper configuration action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="contextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddLiteRepository(this IRepositoryBuilder builder,
			string repositoryName, Type contextType, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder);
			Guard.Against.NullOrWhiteSpace(repositoryName);
			Guard.Against.Null(configure);
			Guard.Against.False(contextType.IsAssignableTo(typeof(LiteContext)),
				message: $"The context type must inherit from '{nameof(LiteContext)}'.");

			BsonMapper.Global.UseRepositoryDefaults();

			builder.Services.AddSingleton<DatabaseProvider>();
			builder.Services.AddSingleton<SequentialGuidGenerator>();
			builder.Services.AddScoped(contextType);
			builder.Services.AddScoped<LiteContextProvider>();
			builder.Services.AddNamedTransient<IUnitOfWork>(serviceBuilder =>
			{
				serviceBuilder.AddNameFor<LiteUnitOfWork>(repositoryName);
			});

			return builder.AddRepository(repositoryName, typeof(LiteRepository<,>), contextType, configure);
		}
	}
}
