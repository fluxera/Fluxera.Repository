namespace Fluxera.Repository.MongoDB
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.IdGenerators;
	using global::MongoDB.Bson.Serialization.Serializers;

	internal sealed class CustomStringObjectIdIdGeneratorConvention : ConventionBase, IPostProcessingConvention
	{
		public void PostProcess(BsonClassMap classMap)
		{
			BsonMemberMap idMemberMap = classMap.IdMemberMap;
			if((idMemberMap == null) || (idMemberMap.IdGenerator != null))
			{
				return;
			}

			if(idMemberMap.MemberType == typeof(string))
			{
				idMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
			}
			else if(idMemberMap.MemberType == typeof(Guid))
			{
				idMemberMap.SetIdGenerator(CombGuidGenerator.Instance);
			}
			//else if(idMemberMap.MemberType == typeof(ObjectId))
			//{
			//	idMemberMap.SetIdGenerator(ObjectIdGenerator.Instance);
			//}
			else
			{
				throw new InvalidOperationException("The MongoDB repository only supports guid or string as type for keys.");
			}

			idMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
		}
	}
}
