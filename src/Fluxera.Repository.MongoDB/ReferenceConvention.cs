namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Reflection;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.Serializers;

	internal sealed class ReferenceConvention : ConventionBase, IMemberMapConvention
	{
		/// <inheritdoc />
		public void Apply(BsonMemberMap memberMap)
		{
			Type memberType = memberMap.MemberType;
			ReferenceAttribute attribute = memberMap.MemberInfo.GetCustomAttribute<ReferenceAttribute>();
			bool isReference = attribute is not null;

			if(isReference)
			{
				string databaseName = attribute.StorageName;
				string collectionName = attribute.ReferencedEntityName.Pluralize();

				if(memberType == typeof(string))
				{
					memberMap.SetSerializer(new StringReferenceSerializer(databaseName, collectionName));
				}
				else if(memberType == typeof(Guid))
				{
					memberMap.SetSerializer(new GuidReferenceSerializer(databaseName, collectionName));
				}
				else if(memberType == typeof(Guid?))
				{
					memberMap.SetSerializer(new NullableSerializer<Guid>(new GuidReferenceSerializer(databaseName, collectionName)));
				}
				else if(memberType.IsStronglyTypedId())
				{
					Type valueType = memberType.GetValueType();
					Type serializerTypeTemplate = typeof(StronglyTypedIdReferenceSerializer<,>);
					Type serializerType = serializerTypeTemplate.MakeGenericType(memberType, valueType);

					IBsonSerializer serializer = (IBsonSerializer)Activator.CreateInstance(serializerType, new object[] { databaseName, collectionName });
					memberMap.SetSerializer(serializer);
				}
				else
				{
					throw new InvalidOperationException("The MongoDB repository only supports Guid or string as type for references.");
				}
			}
		}
	}
}
