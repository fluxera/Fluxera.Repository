namespace Fluxera.Repository.InMemory
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The extension methods to configure an in-memory repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Add an in-memory repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddInMemoryRepository<TContext>(this IRepositoryBuilder builder,
			Action<IRepositoryOptionsBuilder> configure)
			where TContext : InMemoryContext
		{
			return builder.AddInMemoryRepository(typeof(TContext), configure);
		}

		/// <summary>
		///     Add an in-memory repository for the "Default" repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="dbContextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddInMemoryRepository(this IRepositoryBuilder builder,
			Type dbContextType, Action<IRepositoryOptionsBuilder> configure)
		{
			return builder.AddInMemoryRepository("Default", dbContextType, configure);
		}

		/// <summary>
		///     Adds an in-memory storage repository for the given repository name. The repository
		///     options are configured using a options builder configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddInMemoryRepository<TContext>(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
			where TContext : InMemoryContext
		{
			return builder.AddInMemoryRepository(repositoryName, typeof(TContext), configure);
		}

		/// <summary>
		///     Adds an in-memory storage repository for the given repository name. The repository
		///     options are configured using a options builder configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="contextType"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddInMemoryRepository(this IRepositoryBuilder builder,
			string repositoryName, Type contextType, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));
			Guard.Against.False(contextType.IsAssignableTo(typeof(InMemoryContext)),
				message: $"The context type must inherit from '{nameof(InMemoryContext)}'.");

			builder.Services.AddSingleton<SequentialGuidGenerator>();
			builder.Services.AddScoped(contextType);
			builder.Services.AddScoped<InMemoryContextProvider>();
			builder.Services.AddNamedTransient<IUnitOfWork>(serviceBuilder =>
			{
				serviceBuilder.AddNameFor<InMemoryUnitOfWork>(repositoryName);
			});

			return builder.AddRepository(repositoryName, typeof(InMemoryRepository<,>), contextType, configure);
		}
	}
}
