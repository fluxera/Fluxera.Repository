namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.StronglyTypedId;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata;

	/// <summary>
	///     Extension methods for the <see cref="ModelBuilder" /> type.
	/// </summary>
	[PublicAPI]
	public static class ModelBuilderExtensions
	{
		/// <summary>
		///     Configure the model builder to use the <see cref="StronglyTypedIdValueGenerator{TStronglyTypedId,TValue}" />.
		/// </summary>
		/// <param name="modelBuilder"></param>
		public static void UseStronglyTypedIdValueGenerator(this ModelBuilder modelBuilder)
		{
			Guard.Against.Null(modelBuilder);

			foreach(IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
			{
				IEnumerable<PropertyInfo> properties = entityType
					.ClrType
					.GetProperties()
					.Where(propertyInfo => propertyInfo.PropertyType.IsStronglyTypedId());

				foreach(PropertyInfo property in properties)
				{
					Type idType = property.PropertyType;
					Type valueType = idType.GetValueType();

					Type generatorTypeTemplate = typeof(StronglyTypedIdValueGenerator<,>);
					Type generatorType = generatorTypeTemplate.MakeGenericType(idType, valueType);

					modelBuilder
						.Entity(entityType.ClrType)
						.Property(property.Name)
						.HasValueGenerator(generatorType);
				}
			}
		}

		/// <summary>
		///     Configure the model builder to use the <see cref="SequentialGuidStringValueGenerator" /> for string IDs.
		/// </summary>
		/// <param name="modelBuilder"></param>
		public static void UseSequentialGuidStringIdValueGenerator(this ModelBuilder modelBuilder)
		{
			Guard.Against.Null(modelBuilder);

			foreach(IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
			{
				IEnumerable<PropertyInfo> properties = entityType
					.ClrType
					.GetProperties()
					.Where(propertyInfo => propertyInfo.PropertyType == typeof(string) && propertyInfo.Name == "ID");

				foreach(PropertyInfo property in properties)
				{
					modelBuilder
						.Entity(entityType.ClrType)
						.Property(property.Name)
						.HasValueGenerator<SequentialGuidStringValueGenerator>();
				}
			}
		}

		public static void UseReferences(this ModelBuilder modelBuilder)
		{
			IEnumerable<IMutableForeignKey> foreignKeys = modelBuilder.Model
				.GetEntityTypes()
				.Where(e => !e.IsOwned() && e.ClrType.IsAggregateRoot())
				.SelectMany(e => e.GetForeignKeys());

			foreach(IMutableForeignKey relationship in foreignKeys)
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}
	}
}
