// ReSharper disable AssignNullToNotNullAttribute

namespace Fluxera.Repository
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Extension methods for converting key values to expressions.
	/// </summary>
	[PublicAPI]
	public static class KeyExtensions
	{
		/// <summary>
		///     Creates an <see cref="Expression" /> in the form of <c>x => x.ID == id</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Expression<Func<TEntity, bool>> CreatePrimaryKeyPredicate<TEntity, TKey>(this TKey id)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			PropertyInfo primaryKeyProperty = GetPrimaryKeyProperty<TEntity, TKey>();

			ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
			Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(
				Expression.Equal(
					Expression.PropertyOrField(parameter, primaryKeyProperty.Name),
					Expression.Constant(id)
				),
				parameter);

			return predicate;
		}

		/// <summary>
		///     Creates an <see cref="LambdaExpression" /> in the form of <c>x => x.ID == id</c> for the given ID value.
		/// </summary>
		/// <param name="keyType"></param>
		/// <param name="aggregateRootType"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static LambdaExpression CreatePrimaryKeyPredicate(Type aggregateRootType, Type keyType, object id)
		{
			PropertyInfo primaryKeyProperty = GetPrimaryKeyProperty(aggregateRootType, keyType);

			ParameterExpression parameter = Expression.Parameter(aggregateRootType, "x");
			LambdaExpression predicate = Expression.Lambda(
				Expression.Equal(
					Expression.PropertyOrField(parameter, primaryKeyProperty.Name),
					Expression.Constant(id)
				),
				parameter);

			return predicate;
		}

		private static PropertyInfo GetPrimaryKeyProperty<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Type aggregateRootType = typeof(TEntity);
			Type keyType = typeof(TKey);

			return GetPrimaryKeyProperty(aggregateRootType, keyType);
		}

		private static PropertyInfo GetPrimaryKeyProperty(Type aggregateRootType, Type keyType)
		{
			Tuple<Type, Type> key = Tuple.Create(aggregateRootType, keyType);

			// Check the cache for already existing property info instance.
			if(PropertyInfoCache.PrimaryKeyDict.TryGetValue(key, out PropertyInfo property))
			{
				return property;
			}

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

			if(propertyInfo.PropertyType != keyType)
			{
				throw new InvalidOperationException($"No property '{keyPropertyName}' found for type '{aggregateRootType}' that has the type {keyType}.");
			}

			PropertyInfoCache.PrimaryKeyDict[key] = propertyInfo;
			return propertyInfo;
		}
	}
}
