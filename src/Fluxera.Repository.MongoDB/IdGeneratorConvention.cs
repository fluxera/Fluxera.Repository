namespace Fluxera.Repository.MongoDB
{
	using System;
	using Fluxera.StronglyTypedId;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.IdGenerators;
	using global::MongoDB.Bson.Serialization.Serializers;

	internal sealed class IdGeneratorConvention : ConventionBase, IPostProcessingConvention
	{
		public void PostProcess(BsonClassMap classMap)
		{
			BsonMemberMap idMemberMap = classMap.IdMemberMap;
			if(idMemberMap == null || idMemberMap.IdGenerator != null)
			{
				return;
			}

			if(idMemberMap.MemberType == typeof(string))
			{
				idMemberMap
					.SetIdGenerator(StringObjectIdGenerator.Instance)
					.SetSerializer(new StringSerializer(BsonType.ObjectId));
			}
			else if(idMemberMap.MemberType == typeof(Guid))
			{
				idMemberMap
					.SetIdGenerator(CombGuidGenerator.Instance)
					.SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
			}
			else if(idMemberMap.MemberType.IsStronglyTypedId())
			{
				idMemberMap.SetIdGenerator(new StronglyTypedIdGenerator(idMemberMap.MemberType));
			}
			else
			{
				throw new InvalidOperationException("The MongoDB repository only supports Guid or string as type for keys.");
			}
		}
	}
}
