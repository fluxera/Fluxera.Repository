namespace Fluxera.Repository.LiteDB
{
	using System;
	using Fluxera.Guards;
	using global::LiteDB;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		public static IRepositoryBuilder AddLiteRepository(this IRepositoryBuilder builder, string repositoryName,
			Action<IRepositoryOptionsBuilder> configure, Action<BsonMapper> configureMapper)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));

			configureMapper.Invoke(BsonMapper.Global);

			builder.Services.TryAddSingleton<IDatabaseProvider, DatabaseProvider>();
			return builder.AddRepository(repositoryName, typeof(LiteRepository<,>), configure);
		}
	}
}
