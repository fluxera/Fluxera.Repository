namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using global::MongoDB.Driver;
	using JetBrains.Annotations;

	/// <summary>
	///     Serializes repository references as <see cref="MongoDBRef" /> documents.
	/// </summary>
	[PublicAPI]
	public class GuidReferenceSerializer : StructSerializerBase<Guid>
	{
		private readonly string collectionName;
		private readonly string databaseName;

		private readonly MongoDBRefSerializer dbRefSerializer = new MongoDBRefSerializer();

		/// <summary>
		///     Initializes a new instance of the <see cref="GuidReferenceSerializer" /> type.
		/// </summary>
		/// <param name="databaseName"></param>
		/// <param name="collectionName"></param>
		public GuidReferenceSerializer(string databaseName, string collectionName)
		{
			this.databaseName = databaseName;
			this.collectionName = collectionName;
		}

		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Guid value)
		{
			BsonValue id = new BsonBinaryData(value, GuidRepresentation.Standard);
			MongoDBRef dbRef = new MongoDBRef(this.databaseName, this.collectionName, id);

			this.dbRefSerializer.Serialize(context, args, dbRef);
		}

		/// <inheritdoc />
		public override Guid Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			MongoDBRef dbRef = this.dbRefSerializer.Deserialize(context, args);
			return dbRef.Id.AsGuid;
		}
	}
}
