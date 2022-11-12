namespace Fluxera.Repository.MongoDB
{
	using Fluxera.Enumeration.MongoDB;
	using Fluxera.Guards;
	using Fluxera.Spatial.MongoDB;
	using Fluxera.StronglyTypedId.MongoDB;
	using Fluxera.Temporal.MongoDB;
	using Fluxera.ValueObject.MongoDB;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization.Conventions;

	/// <summary>
	///     Extension methods for the <see cref="ConventionPack" /> type.
	/// </summary>
	public static class ConventionPackExtensions
	{
		/// <summary>
		///     Configure the convention pack with default settings.
		/// </summary>
		/// <param name="conventionPack"></param>
		/// <returns></returns>
		public static ConventionPack UseRepositoryDefaults(this ConventionPack conventionPack)
		{
			Guard.Against.Null(conventionPack);

			conventionPack.Add(new NamedIdMemberConvention("ID"));
			conventionPack.Add(new IdGeneratorConvention());
			conventionPack.Add(new ReferenceConvention());
			conventionPack.Add(new EnumRepresentationConvention(BsonType.String));
			conventionPack.Add(new CamelCaseElementNameConvention());
			conventionPack.Add(new IgnoreExtraElementsConvention(true));
			conventionPack.Add(new NamedExtraElementsMemberConvention("ExtraElements"));
			conventionPack.Add(new EntitiesNotSupportedConvention());

			conventionPack.UseSpatial();
			conventionPack.UseTemporal();
			conventionPack.UseEnumeration();
			conventionPack.UsePrimitiveValueObject();
			conventionPack.UseStronglyTypedId();

			return conventionPack;
		}
	}
}
