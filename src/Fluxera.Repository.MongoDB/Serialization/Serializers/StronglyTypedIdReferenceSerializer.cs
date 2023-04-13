namespace Fluxera.Repository.MongoDB.Serialization.Serializers
{
	using System;
	using Fluxera.Repository.MongoDB;
	using Fluxera.StronglyTypedId;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using global::MongoDB.Driver;

	/// <summary>
	///     Serializes repository references as <see cref="MongoDBRef" /> documents.
	/// </summary>
	public class StronglyTypedIdReferenceSerializer<TStronglyTypedId, TValue> : SerializerBase<TStronglyTypedId>
		where TStronglyTypedId : StronglyTypedId<TStronglyTypedId, TValue>
		where TValue : notnull, IComparable
	{
		private readonly IBsonSerializer serializer;

		/// <summary>
		///     Initializes a new instance of the <see cref="StronglyTypedIdReferenceSerializer{TStronglyTypedId, TValue}" /> type.
		/// </summary>
		/// <param name="databaseName"></param>
		/// <param name="collectionName"></param>
		public StronglyTypedIdReferenceSerializer(string databaseName, string collectionName)
		{
			if(typeof(TValue) == typeof(string))
			{
				this.serializer = new StringReferenceSerializer(databaseName, collectionName);
			}
			else if(typeof(TValue) == typeof(Guid))
			{
				this.serializer = new GuidReferenceSerializer(databaseName, collectionName);
			}
			else
			{
				throw new InvalidOperationException("The MongoDB repository only supports Guid or string as type for references.");
			}
		}

		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TStronglyTypedId value)
		{
			this.serializer.Serialize(context, args, value.Value);
		}

		/// <inheritdoc />
		public override TStronglyTypedId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			TStronglyTypedId id = null;
			object value = this.serializer.Deserialize(context, args);

			if(value is not null)
			{
				id = (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), new object[] { value });
			}

			return id;
		}
	}
}
