namespace Fluxera.Repository.MongoDB.Serialization.Serializers
{
	using System;
	using Fluxera.Entity;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using global::MongoDB.Driver;
	using JetBrains.Annotations;

	/// <summary>
	///     Serializes repository references as <see cref="MongoDBRef" /> documents.
	/// </summary>
	[PublicAPI]
	public class AggregateRootReferenceSerializer<TEntity, TValue> : SerializerBase<TEntity>
		where TEntity : Entity<TEntity, TValue>
		where TValue : IComparable<TValue>, IEquatable<TValue>
	{
		private readonly IBsonSerializer innerSerializer;

		/// <summary>
		///     Initializes a new instance of the <see cref="AggregateRootReferenceSerializer{TAggregateRoot, TValue}" /> type.
		/// </summary>
		/// <param name="innerSerializer"></param>
		public AggregateRootReferenceSerializer(IBsonSerializer innerSerializer)
		{
			this.innerSerializer = innerSerializer;
		}

		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEntity value)
		{
			if(value is null || value.ID is null)
			{
				context.Writer.WriteNull();
			}
			else
			{
				TValue id = value.ID;
				this.innerSerializer.Serialize(context, args, id);
			}
		}

		/// <inheritdoc />
		public override TEntity Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			object id = this.innerSerializer.Deserialize(context, args);
			TEntity instance = null;

			if(id is not null)
			{
				instance = Activator.CreateInstance<TEntity>();
				instance.ID = (TValue)id;
			}

			return instance;
		}
	}
}
