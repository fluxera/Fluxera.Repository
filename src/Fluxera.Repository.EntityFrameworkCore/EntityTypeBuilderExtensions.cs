namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Entity;
	using Fluxera.Enumeration.EntityFrameworkCore;
	using Fluxera.Guards;
	using Fluxera.StronglyTypedId;
	using Fluxera.StronglyTypedId.EntityFrameworkCore;
	using Fluxera.ValueObject.EntityFrameworkCore;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	/// <summary>
	///     Extension methods for the <see cref="ModelBuilder" /> type.
	/// </summary>
	[PublicAPI]
	public static class EntityTypeBuilderExtensions
	{
		/// <summary>
		///     Configure the <see cref="EntityTypeBuilder" /> with default settings.
		/// </summary>
		/// <param name="entityTypeBuilder"></param>
		/// <returns></returns>
		public static void UseRepositoryDefaults(this EntityTypeBuilder entityTypeBuilder)
		{
			Guard.Against.Null(entityTypeBuilder);

			entityTypeBuilder.UseEnumeration();
			entityTypeBuilder.UsePrimitiveValueObject();
			entityTypeBuilder.UseStronglyTypedId();
			entityTypeBuilder.UseStronglyTypedIdValueGenerator();
			entityTypeBuilder.UseSequentialGuidStringIdValueGenerator();
			entityTypeBuilder.UseReferences();
		}

		/// <summary>
		///     Configure the <see cref="EntityTypeBuilder" /> to use the
		///     <see cref="StronglyTypedIdValueGenerator{TStronglyTypedId,TValue}" />.
		/// </summary>
		/// <param name="entityTypeBuilder"></param>
		public static void UseStronglyTypedIdValueGenerator(this EntityTypeBuilder entityTypeBuilder)
		{
			Guard.Against.Null(entityTypeBuilder);

			IEnumerable<PropertyInfo> properties = entityTypeBuilder.Metadata
				.ClrType
				.GetProperties()
				.Where(propertyInfo => propertyInfo.PropertyType.IsStronglyTypedId());

			foreach(PropertyInfo property in properties)
			{
				Type idType = property.PropertyType;
				Type valueType = idType.GetStronglyTypedIdValueType();

				Type generatorTypeTemplate = typeof(StronglyTypedIdValueGenerator<,>);
				Type generatorType = generatorTypeTemplate.MakeGenericType(idType, valueType);

				entityTypeBuilder
					.Property(property.Name)
					.HasValueGenerator(generatorType);
			}
		}

		/// <summary>
		///     Configure the <see cref="EntityTypeBuilder" /> to use the <see cref="SequentialGuidStringValueGenerator" />
		///		for string IDs.
		/// </summary>
		/// <param name="entityTypeBuilder"></param>
		public static void UseSequentialGuidStringIdValueGenerator(this EntityTypeBuilder entityTypeBuilder)
		{
			Guard.Against.Null(entityTypeBuilder);

			IEnumerable<PropertyInfo> properties = entityTypeBuilder.Metadata
				.ClrType
				.GetProperties()
				.Where(propertyInfo => propertyInfo.PropertyType == typeof(string) && propertyInfo.Name == "ID");

			foreach(PropertyInfo property in properties)
			{
				entityTypeBuilder
					.Property(property.Name)
					.HasValueGenerator<SequentialGuidStringValueGenerator>();
			}
		}

		/// <summary>
		///     Disables delete cascading for references aggregate roots.
		/// </summary>
		/// <param name="entityTypeBuilder"></param>
		public static void UseReferences(this EntityTypeBuilder entityTypeBuilder)
		{
			IMutableEntityType entityType = entityTypeBuilder.Metadata;

			if(!entityType.IsOwned() && entityType.ClrType.IsEntity())
			{
				// TODO
				//IEnumerable<IMutableForeignKey> foreignKeys = entityType.GetForeignKeys();

				//foreach(IMutableForeignKey relationship in foreignKeys)
				//{
				//	relationship.DeleteBehavior = DeleteBehavior.Restrict;
				//}
			}
		}
	}
}
