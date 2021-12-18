namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.Serializers;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		public static IRepositoryBuilder AddMongoRepository(this IRepositoryBuilder builder, string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions,
			Action<ConventionPack>? configureConventions = null)
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

			ConventionPack pack = new ConventionPack
			{
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
