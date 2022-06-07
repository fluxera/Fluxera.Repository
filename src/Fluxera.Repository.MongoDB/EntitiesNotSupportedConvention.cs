namespace Fluxera.Repository.MongoDB
{
	using System;
	using Fluxera.Entity;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;

	internal sealed class EntitiesNotSupportedConvention : ConventionBase, IClassMapConvention, IMemberMapConvention
	{
		/// <inheritdoc />
		public void Apply(BsonClassMap classMap)
		{
			Type classType = classMap.ClassType;

			if(classType.IsEntity() && !classType.IsAggregateRoot())
			{
				throw new NotSupportedException($"Entity types are not supported. Type={classType.Name}");
			}
		}

		/// <inheritdoc />
		public void Apply(BsonMemberMap memberMap)
		{
			Type memberType = memberMap.MemberType;

			if(memberType.IsEntity() && !memberType.IsAggregateRoot())
			{
				throw new NotSupportedException($"Entity types are not supported. Type={memberType.Name}");
			}
		}
	}
}
