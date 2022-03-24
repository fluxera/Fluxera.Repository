//namespace Fluxera.Repository.MongoDB
//{
//	using System;
//	using System.Reflection;
//	using Fluxera.ComponentModel.Annotations;
//	using Fluxera.Utilities.Extensions;
//	using global::MongoDB.Bson;
//	using global::MongoDB.Bson.Serialization;
//	using global::MongoDB.Bson.Serialization.Conventions;
//	using global::MongoDB.Bson.Serialization.Serializers;

//	internal sealed class DateTimeOffsetConvention : ConventionBase, IMemberMapConvention
//	{
//		/// <inheritdoc />
//		public void Apply(BsonMemberMap memberMap)
//		{
//			Type originalMemberType = memberMap.MemberType;
//			Type memberType = originalMemberType.UnwrapNullableType();

//			MemberInfo member = memberMap.MemberInfo;

//			if(memberType == typeof(DateTimeOffset))
//			{
//				bool dateOnly = member.IsDefined(typeof(DateOnlyAttribute));

//				BsonType representation = dateOnly ? BsonType.String : BsonType.Document;

//				DateTimeOffsetSerializer dateTimeOffsetSerializer = new DateTimeOffsetSerializer(representation);

//				if(dateOnly)
//				{
//					DateTimeSerializer dateTimeSerializer = DateTimeSerializer.UtcInstance
//						.WithRepresentation(representation)
//						.WithDateOnly(true);

//					dateTimeOffsetSerializer = new DateOnlyDateTimeOffsetSerializer(dateTimeSerializer);
//				}

//				if(originalMemberType.IsNullable())
//				{
//					memberMap.SetSerializer(new NullableSerializer<DateTimeOffset>(dateTimeOffsetSerializer));
//				}
//				else
//				{
//					memberMap.SetSerializer(dateTimeOffsetSerializer);
//				}
//			}
//		}
//	}
//}


