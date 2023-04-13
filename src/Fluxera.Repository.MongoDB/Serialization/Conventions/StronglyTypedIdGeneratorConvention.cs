namespace Fluxera.Repository.MongoDB.Serialization.Conventions
{
	using System;
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;

	internal sealed class StronglyTypedIdGeneratorConvention : ConventionBase, IPostProcessingConvention
	{
		public void PostProcess(BsonClassMap classMap)
		{
			BsonMemberMap idMemberMap = classMap.IdMemberMap;
			if(idMemberMap == null || idMemberMap.IdGenerator != null)
			{
				return;
			}

			Type idMemberType = idMemberMap.MemberType.UnwrapNullableType();
			if(idMemberType.IsStronglyTypedId())
			{
				idMemberMap.SetIdGenerator(new StronglyTypedIdGenerator(idMemberMap.MemberType));
			}
		}
	}
}
