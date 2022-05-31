namespace Fluxera.Repository.InMemory
{
	using System;
	using Fluxera.Guards;
	using Fluxera.Repository.LiteDB;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     The extension methods to configure an in-memory repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Adds an in-memory storage repository for the given repository name. The repository
		///     options are configured using a options builder configure action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddInMemoryRepository(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));

			builder.Services.AddSingleton<SequentialGuidGenerator>();

			return builder.AddRepository(repositoryName, typeof(InMemoryRepository<,>), configure);
		}
	}
}
