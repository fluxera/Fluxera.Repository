namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.Serializers;

	internal sealed class GuidAsStringRepresentationConvention : ConventionBase, IMemberMapConvention
	{
		public void Apply(BsonMemberMap memberMap)
		{
			if(memberMap.MemberType == typeof(Guid))
			{
				memberMap.SetSerializer(new GuidSerializer(BsonType.String));
			}
			else if(memberMap.MemberType == typeof(Guid?))
			{
				memberMap.SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)));
			}
		}
	}
}
