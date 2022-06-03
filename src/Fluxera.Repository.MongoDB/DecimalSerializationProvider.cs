namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using JetBrains.Annotations;

	/// <inheritdoc />
	[PublicAPI]
	public class DecimalSerializationProvider : IBsonSerializationProvider
	{
		private static readonly DecimalSerializer DecimalSerializer = new DecimalSerializer(BsonType.Decimal128);
		private static readonly NullableSerializer<decimal> NullableDecimalSerializer = new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128));

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if(type == typeof(decimal))
			{
				return DecimalSerializer;
			}

			if(type == typeof(decimal?))
			{
				return NullableDecimalSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
