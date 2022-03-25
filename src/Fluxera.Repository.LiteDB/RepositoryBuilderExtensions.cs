namespace Fluxera.Repository.LiteDB
{
	using System;
	using Fluxera.Guards;
	using global::LiteDB;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	/// <summary>
	///     The extensions methods to configure a LiteDb repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Adds a LiteDB repository for the given repository name. The repository options
		///     are configured using the options builder configure action. Additional LiteDB
		///     related mappings can be configures using the mapper configuration action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <param name="configureMapper"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddLiteRepository(this IRepositoryBuilder builder, string repositoryName,
			Action<IRepositoryOptionsBuilder> configure, Action<BsonMapper> configureMapper)
		{
			Guard.Against.Null(builder, nameof(builder));
			Guard.Against.NullOrWhiteSpace(repositoryName, nameof(repositoryName));
			Guard.Against.Null(configure, nameof(configure));

			// TODO: Configure this.
			//var mapper = BsonMapper.Global;
			//mapper.Entity<MyEntity>()
			//	.Id(x => x.ID) // set your document ID

			configureMapper.Invoke(BsonMapper.Global);

			builder.Services.TryAddSingleton<IDatabaseProvider, DatabaseProvider>();
			return builder.AddRepository(repositoryName, typeof(LiteRepository<,>), configure);
		}
	}
}
