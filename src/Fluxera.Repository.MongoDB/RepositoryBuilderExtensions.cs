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
		static RepositoryBuilderExtensions()
		{
#pragma warning disable CS0618
			BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618

			BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new DecimalSerializationProvider());
		}

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
			ConventionPack pack = new ConventionPack
			{
				new NamedIdMemberConvention("ID"),
				new IdGeneratorConvention(),
				new ReferenceConvention(),
				new EnumRepresentationConvention(BsonType.String),
				new CamelCaseElementNameConvention(),
				new IgnoreExtraElementsConvention(true),
				new NamedExtraElementsMemberConvention("ExtraElements"),
				new EntitiesNotSupportedConvention()
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
