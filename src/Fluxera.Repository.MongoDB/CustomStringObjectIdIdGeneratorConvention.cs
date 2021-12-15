namespace Fluxera.Repository.MongoDB
{
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
				idMemberMap
					.SetIdGenerator(StringObjectIdGenerator.Instance)
					.SetSerializer(new StringSerializer(BsonType.ObjectId));
			}
		}
	}
}
