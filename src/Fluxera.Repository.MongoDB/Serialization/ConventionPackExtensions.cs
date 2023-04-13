namespace Fluxera.Repository.MongoDB.Serialization
{
	using Fluxera.Enumeration.MongoDB;
	using Fluxera.Guards;
	using Fluxera.Repository.MongoDB.Serialization.Conventions;
	using Fluxera.Spatial.MongoDB;
	using Fluxera.StronglyTypedId.MongoDB;
	using Fluxera.ValueObject.MongoDB;
	using global::MongoDB.Bson.Serialization.Conventions;
	using JetBrains.Annotations;

	/// <summary>
	///     Extension methods for the <see cref="ConventionPack" /> type.
	/// </summary>
	[PublicAPI]
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

			conventionPack.Add(new CheckIdTypeConvention());
			conventionPack.Add(new StronglyTypedIdGeneratorConvention());
			conventionPack.Add(new ReferenceConvention());
			conventionPack.Add(new EntitiesNotSupportedConvention());

			conventionPack.UseSpatial();
			conventionPack.UseEnumeration();
			conventionPack.UsePrimitiveValueObject();
			conventionPack.UseStronglyTypedId();

			return conventionPack;
		}
	}
}
