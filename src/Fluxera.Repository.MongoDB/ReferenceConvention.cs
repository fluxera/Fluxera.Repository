namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Reflection;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;
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

			if(isReference && !memberType.IsAggregateRoot())
			{
				string databaseName = attribute.StorageName;
				string collectionName = attribute.ReferencedEntityName.Pluralize();

				IBsonSerializer serializer = this.GetReferenceSerializer(memberType, databaseName, collectionName);
				memberMap.SetSerializer(serializer);
			}

			if(memberType.IsAggregateRoot())
			{
				string databaseName = attribute?.StorageName;
				string collectionName = memberType.Name.Pluralize();

				Type idType = memberType.BaseType?.GenericTypeArguments[1];
				IBsonSerializer referenceSerializer = this.GetReferenceSerializer(idType, databaseName, collectionName);

				Type serializerTypeTemplate = typeof(AggregateRootReferenceSerializer<,>);
				Type serializerType = serializerTypeTemplate.MakeGenericType(memberType, idType);

				IBsonSerializer serializer = (IBsonSerializer)Activator.CreateInstance(serializerType, new object[] { referenceSerializer });
				memberMap.SetSerializer(serializer);
			}
		}

		private IBsonSerializer GetReferenceSerializer(Type memberType, string databaseName, string collectionName)
		{
			IBsonSerializer serializer;

			if(memberType == typeof(string))
			{
				serializer = new StringReferenceSerializer(databaseName, collectionName);
			}
			else if(memberType == typeof(Guid))
			{
				serializer = new GuidReferenceSerializer(databaseName, collectionName);
			}
			else if(memberType == typeof(Guid?))
			{
				serializer = new NullableSerializer<Guid>(new GuidReferenceSerializer(databaseName, collectionName));
			}
			else if(memberType.IsStronglyTypedId())
			{
				Type valueType = memberType.GetValueType();
				Type serializerTypeTemplate = typeof(StronglyTypedIdReferenceSerializer<,>);
				Type serializerType = serializerTypeTemplate.MakeGenericType(memberType, valueType);

				serializer = (IBsonSerializer)Activator.CreateInstance(serializerType, new object[] { databaseName, collectionName });
			}
			else if(memberType.IsEnumerable())
			{
				Type elementType = memberType.GenericTypeArguments[0];

				serializer = BsonSerializer.SerializerRegistry.GetSerializer(memberType);
				IChildSerializerConfigurable listSerializer = (IChildSerializerConfigurable)serializer;
				IChildSerializerConfigurable itemSerializer = (IChildSerializerConfigurable)listSerializer.ChildSerializer;

				if(elementType == typeof(string))
				{
					IBsonSerializer newListSerializer = itemSerializer.WithChildSerializer(new StringReferenceSerializer(databaseName, collectionName));
					serializer = listSerializer.WithChildSerializer(newListSerializer);
				}
				else if(elementType == typeof(Guid))
				{
					IBsonSerializer newListSerializer = itemSerializer.WithChildSerializer(new GuidReferenceSerializer(databaseName, collectionName));
					serializer = listSerializer.WithChildSerializer(newListSerializer);
				}
				else if(elementType == typeof(Guid?))
				{
					IBsonSerializer newListSerializer = itemSerializer.WithChildSerializer(new NullableSerializer<Guid>(new GuidReferenceSerializer(databaseName, collectionName)));
					serializer = listSerializer.WithChildSerializer(newListSerializer);
				}
				else if(elementType.IsStronglyTypedId())
				{
					Type valueType = elementType.GetValueType();
					Type serializerTypeTemplate = typeof(StronglyTypedIdReferenceSerializer<,>);
					Type serializerType = serializerTypeTemplate.MakeGenericType(elementType, valueType);

					IBsonSerializer stronglyTypedIdSerializer = (IBsonSerializer)Activator.CreateInstance(serializerType, new object[] { databaseName, collectionName });

					IBsonSerializer newListSerializer = itemSerializer.WithChildSerializer(stronglyTypedIdSerializer);
					serializer = listSerializer.WithChildSerializer(newListSerializer);
				}
				else if(elementType.IsAggregateRoot())
				{
					Type idType = elementType.BaseType?.GenericTypeArguments[1];
					IBsonSerializer referenceSerializer = this.GetReferenceSerializer(idType, databaseName, collectionName);

					Type serializerTypeTemplate = typeof(AggregateRootReferenceSerializer<,>);
					Type serializerType = serializerTypeTemplate.MakeGenericType(elementType, idType);

					IBsonSerializer aggregateRootSerializer = (IBsonSerializer)Activator.CreateInstance(serializerType, new object[] { referenceSerializer });

					IBsonSerializer newListSerializer = itemSerializer.WithChildSerializer(aggregateRootSerializer);
					serializer = listSerializer.WithChildSerializer(newListSerializer);
				}
				else
				{
					throw new InvalidOperationException("The MongoDB repository only supports Guid or string as type for references.");
				}
			}
			else
			{
				throw new InvalidOperationException("The MongoDB repository only supports Guid or string as type for references.");
			}

			return serializer;
		}
	}
}
