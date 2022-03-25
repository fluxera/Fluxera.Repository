namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization.Conventions;
	using JetBrains.Annotations;

	/// <summary>
	///     The extensions methods for configuring a MongoDB repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		/// <summary>
		///     Adds a MongoDb repository for the given repository name. The repository options
		///     are configured using the options builder config action. Additional MongoDB related
		///     conventions can be registered using the configure convention action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configureOptions"></param>
		/// <param name="configureConventions"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddMongoRepository(this IRepositoryBuilder builder, string repositoryName,
			Action<IRepositoryOptionsBuilder> configureOptions, Action<ConventionPack>? configureConventions = null)
		{
			ConventionPack pack = new ConventionPack
			{
				new NamedIdMemberConvention("ID"),
				new IdGeneratorConvention(),
				new GuidAsStringRepresentationConvention(),
				new EnumRepresentationConvention(BsonType.String),
				new CamelCaseElementNameConvention(),
				new IgnoreExtraElementsConvention(true),
				new NamedExtraElementsMemberConvention("ExtraElements")
			};

			configureConventions?.Invoke(pack);

			ConventionRegistry.Register("ConventionPack", pack, _ => true);

			return builder.AddRepository(repositoryName, typeof(MongoRepository<,>), configureOptions);
		}
	}
}
