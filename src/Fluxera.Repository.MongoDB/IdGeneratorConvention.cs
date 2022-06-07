namespace Fluxera.Repository.MongoDB
{
	using System;
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
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

			Type idMemberType = idMemberMap.MemberType.UnwrapNullableType();
			if(idMemberType == typeof(string))
			{
				idMemberMap
					.SetIdGenerator(StringObjectIdGenerator.Instance)
					.SetSerializer(new StringSerializer(BsonType.ObjectId));
			}
			else if(idMemberType == typeof(Guid))
			{
				idMemberMap
					.SetIdGenerator(CombGuidGenerator.Instance)
					.SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
			}
			else if(idMemberType.IsStronglyTypedId())
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
