namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The extensions methods to configure a EF repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Add an EF repository for the given repository name. The repository options
		///     are configured using the given configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddEntityFrameworkRepository(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));

			builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();

			return builder.AddRepository(repositoryName, typeof(EntityFrameworkCoreRepository<,>), configure);
		}
	}
}
