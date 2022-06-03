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
		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if(type == typeof(Guid))
			{
				return new GuidSerializer(GuidRepresentation.Standard);
			}

			return null;
		}
	}
}
