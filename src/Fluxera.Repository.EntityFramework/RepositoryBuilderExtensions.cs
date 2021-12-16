namespace Fluxera.Repository.EntityFramework
{
	using System;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		public static IRepositoryBuilder AddEntityFrameworkRepository(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));

			builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();

			return builder.AddRepository(repositoryName, typeof(EntityFrameworkRepository<>), configure);
		}
	}
}
