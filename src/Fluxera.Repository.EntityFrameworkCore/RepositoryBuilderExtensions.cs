namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The extensions methods to configure a EF repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Add an EFCore repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddEntityFrameworkRepository<TContext>(this IRepositoryBuilder builder,
			Action<IRepositoryOptionsBuilder> configure)
			where TContext : DbContext
		{
			return builder.AddEntityFrameworkRepository(typeof(TContext), configure);
		}

		/// <summary>
		///     Add an EFCore repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="dbContextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddEntityFrameworkRepository(this IRepositoryBuilder builder,
			Type dbContextType, Action<IRepositoryOptionsBuilder> configure)
		{
			return builder.AddEntityFrameworkRepository("Default", dbContextType, configure);
		}

		/// <summary>
		///     Add an EFCore repository for the given repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddEntityFrameworkRepository<TContext>(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
			where TContext : DbContext
		{
			return builder.AddEntityFrameworkRepository(repositoryName, typeof(TContext), configure);
		}

		/// <summary>
		///     Add an EFCore repository for the given repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="contextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddEntityFrameworkRepository(this IRepositoryBuilder builder,
			string repositoryName, Type contextType, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder);
			Guard.Against.NullOrWhiteSpace(repositoryName);
			Guard.Against.Null(configure);

			builder.Services.AddScoped<DbContextProvider>();
			builder.Services.AddNamedTransient<IUnitOfWork>(serviceBuilder =>
			{
				serviceBuilder.AddNameFor<EntityFrameworkCoreUnitOfWork>(repositoryName);
			});

			return builder.AddRepository(repositoryName, typeof(EntityFrameworkCoreRepository<,>), contextType, configure);
		}
	}
}
