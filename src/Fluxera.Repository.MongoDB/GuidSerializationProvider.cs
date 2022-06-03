namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using JetBrains.Annotations;

	/// <inheritdoc />
	[PublicAPI]
	public class GuidSerializationProvider : IBsonSerializationProvider
	{
		private static readonly GuidSerializer GuidSerializer = new GuidSerializer(GuidRepresentation.Standard);
		private static readonly NullableSerializer<Guid> NullableGuidSerializer = new NullableSerializer<Guid>(new GuidSerializer(GuidRepresentation.Standard));

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if(type == typeof(Guid))
			{
				return GuidSerializer;
			}

			if(type == typeof(Guid?))
			{
				return NullableGuidSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
