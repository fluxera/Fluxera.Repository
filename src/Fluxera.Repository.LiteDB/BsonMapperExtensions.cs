// ReSharper disable AssignNullToNotNullAttribute

namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;
	using Fluxera.Enumeration.LiteDB;
	using Fluxera.Guards;
	using Fluxera.Spatial.LiteDB;
	using Fluxera.StronglyTypedId.LiteDB;
	using Fluxera.Utilities.Extensions;
	using Fluxera.ValueObject.LiteDB;
	using global::LiteDB;
	using JetBrains.Annotations;

	/// <summary>
	///     Extension methods for the <see cref="BsonMapper" /> type.
	/// </summary>
	[PublicAPI]
	public static class BsonMapperExtensions
	{
		/// <summary>
		///     Configure the bson mapper with default settings.
		/// </summary>
		/// <param name="bsonMapper"></param>
		/// <returns></returns>
		public static BsonMapper UseRepositoryDefaults(this BsonMapper bsonMapper)
		{
			Guard.Against.Null(bsonMapper);

			bsonMapper.UseSpatial();
			bsonMapper.UseEnumeration();
			bsonMapper.UsePrimitiveValueObject();
			bsonMapper.UseStronglyTypedId();
			bsonMapper.UseReferences();

			return bsonMapper;
		}

		/// <summary>
		///     Configures the mapper to reference marked properties as DbRef
		///     instead of including the complete document.
		/// </summary>
		/// <param name="bsonMapper"></param>
		/// <returns></returns>
		public static BsonMapper UseReferences(this BsonMapper bsonMapper)
		{
			Guard.Against.Null(bsonMapper);

			IList<Type> aggregateRootTypes = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsEntity())
				.ToList();

			MethodInfo entityMethod = bsonMapper.GetType().GetMethod("Entity", BindingFlags.Public | BindingFlags.Instance);

			foreach(Type aggregateRootType in aggregateRootTypes)
			{
				object entityBuilder = entityMethod?.MakeGenericMethod(aggregateRootType).Invoke(bsonMapper, []);
				MethodInfo idMethod = entityBuilder?.GetType().GetMethod("Id");

				// Configure the ID property to use.
				PropertyInfo keyProperty = GetPrimaryKeyProperty(aggregateRootType);
				LambdaExpression keyPropertyExpression = CreatePropertyExpression(aggregateRootType, keyProperty);
				entityBuilder = idMethod?
					.MakeGenericMethod(keyProperty.PropertyType)
					.Invoke(entityBuilder, [keyPropertyExpression, true]);

				// Configure reference properties.
				foreach(PropertyInfo referenceProperty in EnumerateReferenceProperties(aggregateRootType))
				{
					//ReferenceAttribute attribute = referenceProperty.GetCustomAttribute<ReferenceAttribute>();

					Type propertyType = referenceProperty.PropertyType;

					// Configure entity properties.
					if(propertyType.IsEntity())
					{
						string collectionName = propertyType.Name.Pluralize();
						entityBuilder = ConfigureReferenceProperty(entityBuilder, aggregateRootType, referenceProperty, collectionName);
					}
					else if(propertyType.IsCollection()) // Configure collection properties.
					{
						Type elementType = propertyType.GenericTypeArguments[0];

						// Configure entity properties.
						if(elementType.IsEntity())
						{
							string collectionName = elementType.Name.Pluralize();
							entityBuilder = ConfigureReferenceProperty(entityBuilder, aggregateRootType, referenceProperty, collectionName);
						}
					}
				}
			}

			return bsonMapper;
		}

		private static object ConfigureReferenceProperty(object entityBuilder, Type aggregateRootType, PropertyInfo property, string collectionName)
		{
			MethodInfo dbRefMethod = entityBuilder?.GetType().GetMethod("DbRef");

			Type propertyType = property.PropertyType;

			// Configure the DbRef property to use.
			LambdaExpression referenceExpression = CreatePropertyExpression(aggregateRootType, property);
			entityBuilder = dbRefMethod?
				.MakeGenericMethod(propertyType)
				.Invoke(entityBuilder, [referenceExpression, collectionName]);

			return entityBuilder;
		}

		private static IEnumerable<PropertyInfo> EnumerateReferenceProperties(Type aggregateRootType)
		{
			foreach(PropertyInfo property in aggregateRootType.GetProperties())
			{
				if(property.DefinesAttribute<ReferenceAttribute>())
				{
					yield return property;
				}
			}
		}

		private static LambdaExpression CreatePropertyExpression(Type aggregateRootType, MemberInfo member)
		{
			ParameterExpression parameter = Expression.Parameter(aggregateRootType, "x");

			LambdaExpression expression = Expression.Lambda(
				Expression.Property(parameter, member.Name),
				parameter
			);

			//MethodInfo createExpressionMethod = typeof(BsonMapperExtensions).GetMethod(nameof(CreateExpression), BindingFlags.Static | BindingFlags.NonPublic);
			//LambdaExpression expression = (LambdaExpression)createExpressionMethod?.MakeGenericMethod(aggregateRootType, member.GetMemberType()).Invoke(null, new object[] { aggregateRootType, member });

			return expression;
		}

		//private static Expression<Func<T, TKey>> CreateExpression<T, TKey>(Type aggregateRootType, MemberInfo member)
		//{
		//	ParameterExpression parameter = Expression.Parameter(aggregateRootType, "x");

		//	Expression<Func<T, TKey>> expression = Expression.Lambda<Func<T, TKey>>(
		//		Expression.Property(parameter, member.Name),
		//		parameter
		//	);

		//	return expression;
		//}

		private static PropertyInfo GetPrimaryKeyProperty(Type aggregateRootType)
		{
			const string keyPropertyName = "ID";
			PropertyInfo propertyInfo = aggregateRootType.GetTypeInfo().GetDeclaredProperty(keyPropertyName);
			while(propertyInfo == null && aggregateRootType.GetTypeInfo().BaseType != null)
			{
				aggregateRootType = aggregateRootType.GetTypeInfo().BaseType;
				propertyInfo = aggregateRootType.GetTypeInfo().GetDeclaredProperty(keyPropertyName);
			}

			if(propertyInfo == null)
			{
				throw new InvalidOperationException($"No property '{keyPropertyName}' found for type '{aggregateRootType}'.");
			}

			return propertyInfo;
		}
	}
}
