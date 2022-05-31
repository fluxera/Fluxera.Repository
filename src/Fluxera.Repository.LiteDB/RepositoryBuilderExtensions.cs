namespace Fluxera.Repository.LiteDB
{
	using System;
	using Fluxera.Enumeration.LiteDB;
	using Fluxera.Guards;
	using Fluxera.Spatial.LiteDB;
	using Fluxera.StronglyTypedId.LiteDB;
	using Fluxera.ValueObject.LiteDB;
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
		///     Adds a LiteDB repository for the given repository name. The repository options
		///     are configured using the options builder configure action. Additional LiteDB
		///     related mappings can be configures using the mapper configuration action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddLiteRepository(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configure)
		{
			Guard.Against.Null(builder);
			Guard.Against.NullOrWhiteSpace(repositoryName);
			Guard.Against.Null(configure);

			BsonMapper.Global.UseSpatial();
			//BsonMapper.Global.UseTemporal();
			BsonMapper.Global.UseEnumeration();
			BsonMapper.Global.UsePrimitiveValueObject();
			BsonMapper.Global.UseStronglyTypedId();

			builder.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
			builder.Services.AddSingleton<SequentialGuidGenerator>();
			return builder.AddRepository(repositoryName, typeof(LiteRepository<,>), configure);
		}
	}
}
