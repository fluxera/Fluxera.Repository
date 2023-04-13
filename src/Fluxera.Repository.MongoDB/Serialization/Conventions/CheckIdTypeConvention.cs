namespace Fluxera.Repository.MongoDB.Serialization.Conventions
{
	using System;
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;

	internal sealed class CheckIdTypeConvention : ConventionBase, IPostProcessingConvention
	{
		/// <inheritdoc />
		public void PostProcess(BsonClassMap classMap)
		{
			BsonMemberMap idMemberMap = classMap.IdMemberMap;
			Type idMemberType = idMemberMap.MemberType.UnwrapNullableType();

			if(idMemberType != typeof(string) &&
			   idMemberType != typeof(Guid) &
			   idMemberType != typeof(ObjectId) &
			   !idMemberType.IsStronglyTypedId())
			{
				throw new InvalidOperationException("The MongoDB repository only supports String, Guid or ObjectId as type for keys.");
			}
		}
	}
}
