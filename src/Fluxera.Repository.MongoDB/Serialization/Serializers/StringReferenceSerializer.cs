namespace Fluxera.Repository.MongoDB.Serialization.Serializers
{
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using global::MongoDB.Driver;
	using JetBrains.Annotations;

	/// <summary>
	///     Serializes repository references as <see cref="MongoDBRef" /> documents.
	/// </summary>
	[PublicAPI]
	public class StringReferenceSerializer : SealedClassSerializerBase<string>
	{
		private readonly string collectionName;
		private readonly string databaseName;

		private readonly MongoDBRefSerializer dbRefSerializer = new MongoDBRefSerializer();

		/// <summary>
		///     Initializes a new instance of the <see cref="StringReferenceSerializer" /> type.
		/// </summary>
		/// <param name="databaseName"></param>
		/// <param name="collectionName"></param>
		public StringReferenceSerializer(string databaseName, string collectionName)
		{
			this.databaseName = databaseName;
			this.collectionName = collectionName;
		}

		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
		{
			if(!string.IsNullOrWhiteSpace(value))
			{
				BsonValue id = ObjectId.Parse(value);
				MongoDBRef dbRef = new MongoDBRef(this.databaseName, this.collectionName, id);

				this.dbRefSerializer.Serialize(context, args, dbRef);
			}
			else
			{
				BsonSerializer.Serialize(context.Writer, value);
			}
		}

		/// <inheritdoc />
		protected override string DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			MongoDBRef dbRef = this.dbRefSerializer.Deserialize(context, args);
			return dbRef?.Id?.ToString();
		}
	}
}
