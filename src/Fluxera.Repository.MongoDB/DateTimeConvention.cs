namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Reflection;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.Serializers;

	internal sealed class DateTimeConvention : ConventionBase, IMemberMapConvention
	{
		/// <inheritdoc />
		public void Apply(BsonMemberMap memberMap)
		{
			Type originalMemberType = memberMap.MemberType;
			Type memberType = originalMemberType.UnwrapNullableType();

			MemberInfo member = memberMap.MemberInfo;

			if(memberType == typeof(DateTime))
			{
				bool dateOnly = member.IsDefined(typeof(DateOnlyAttribute));

				BsonType representation = dateOnly ? BsonType.String : BsonType.Document;

				DateTimeSerializer dateTimeSerializer = DateTimeSerializer.UtcInstance
					.WithRepresentation(representation)
					.WithDateOnly(dateOnly);

				if(dateOnly)
				{
					dateTimeSerializer = new DateOnlyDateTimeSerializer(dateTimeSerializer);
				}

				if(originalMemberType.IsNullable())
				{
					memberMap.SetSerializer(new NullableSerializer<DateTime>(dateTimeSerializer));
				}
				else
				{
					memberMap.SetSerializer(dateTimeSerializer);
				}
			}
		}
	}
}
