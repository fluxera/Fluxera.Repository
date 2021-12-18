namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization.Conventions;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		public static IRepositoryBuilder AddMongoRepository(this IRepositoryBuilder builder, string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions,
			Action<ConventionPack>? configureConventions = null)
		{
			ConventionPack pack = new ConventionPack
			{
				new GuidAsStringRepresentationConvention(),
				new EnumRepresentationConvention(BsonType.String),
				new CamelCaseElementNameConvention(),
				new NamedIdMemberConvention("ID"),
				new CustomStringObjectIdIdGeneratorConvention(),
				new DateTimeConvention(),
				new DateTimeOffsetConvention(),
				new IgnoreExtraElementsConvention(true),
				new NamedExtraElementsMemberConvention("ExtraElements"),
			};

			configureConventions?.Invoke(pack);

			ConventionRegistry.Register("ConventionPack", pack, _ => true);

			return builder.AddRepository(repositoryName, typeof(MongoRepository<,>), configureOptions);
		}
	}
}
