namespace Fluxera.Repository.MongoDB
{
	using System;
	using Fluxera.Enumeration.MongoDB;
	using Fluxera.Spatial.MongoDB;
	using Fluxera.StronglyTypedId.MongoDB;
	using Fluxera.Temporal.MongoDB;
	using Fluxera.ValueObject.MongoDB;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using JetBrains.Annotations;

	/// <summary>
	///     The extensions methods for configuring a MongoDB repository.
	/// </summary>
	[PublicAPI]
	public static class RepositoryBuilderExtensions
	{
		private static GuidSerializationProvider serializationProvider;

		/// <summary>
		///     Adds a MongoDb repository for the given repository name. The repository options
		///     are configured using the options builder config action. Additional MongoDB related
		///     conventions can be registered using the configure convention action.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="repositoryName"></param>
		/// <param name="configureOptions"></param>
		/// <returns></returns>
		public static IRepositoryBuilder AddMongoRepository(this IRepositoryBuilder builder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			if(serializationProvider != null)
			{
				serializationProvider = new GuidSerializationProvider();

				BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
				BsonSerializer.RegisterSerializationProvider(serializationProvider);
			}

			ConventionPack pack = new ConventionPack
			{
				new NamedIdMemberConvention("ID"),
				new IdGeneratorConvention(),
				new EnumRepresentationConvention(BsonType.String),
				new CamelCaseElementNameConvention(),
				new IgnoreExtraElementsConvention(true),
				new NamedExtraElementsMemberConvention("ExtraElements")
			};

			pack.UseSpatial();
			pack.UseTemporal();
			pack.UseEnumeration();
			pack.UsePrimitiveValueObject();
			pack.UseStronglyTypedId();

			ConventionRegistry.Register("ConventionPack", pack, _ => true);

			return builder.AddRepository(repositoryName, typeof(MongoRepository<,>), configureOptions);
		}
	}
}
