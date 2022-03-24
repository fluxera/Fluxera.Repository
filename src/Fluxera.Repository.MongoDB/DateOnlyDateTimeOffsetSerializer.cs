//namespace Fluxera.Repository.MongoDB
//{
//	using System;
//	using global::MongoDB.Bson.Serialization;
//	using global::MongoDB.Bson.Serialization.Serializers;

//	internal sealed class DateOnlyDateTimeOffsetSerializer : DateTimeOffsetSerializer
//	{
//		private readonly DateTimeSerializer dateTimeSerializer;

//		public DateOnlyDateTimeOffsetSerializer(DateTimeSerializer dateTimeSerializer)
//		{
//			this.dateTimeSerializer = dateTimeSerializer;
//		}

//		/// <inheritdoc />
//		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
//		{
//			this.dateTimeSerializer.Serialize(context, args, value.Date);
//		}

//		/// <inheritdoc />
//		public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//		{
//			DateTimeOffset dateTimeOffset = this.dateTimeSerializer.Deserialize(context, args);
//			return dateTimeOffset.Date;
//		}
//	}
//}


