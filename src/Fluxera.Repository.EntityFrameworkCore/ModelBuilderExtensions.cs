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

	/// <summary>
	///     Extension methods for the <see cref="ModelBuilder" /> type.
	/// </summary>
	[PublicAPI]
	public static class ModelBuilderExtensions
	{
		/// <summary>
		///     Configure the model builder with default settings.
		/// </summary>
		/// <param name="modelBuilder"></param>
		/// <returns></returns>
		public static ModelBuilder UseRepositoryDefaults(this ModelBuilder modelBuilder)
		{
			Guard.Against.False(modelBuilder.Model.GetEntityTypes().Any(),
				message: "The entities for the DbContext must be added before the call to UseRepositoryDefaults.");

			//modelBuilder.UseSpatial();
			//modelBuilder.UseTemporal();
			modelBuilder.UseEnumeration();
			modelBuilder.UsePrimitiveValueObject();
			modelBuilder.UseStronglyTypedId();
			modelBuilder.UseStronglyTypedIdValueGenerator();
			modelBuilder.UseSequentialGuidStringIdValueGenerator();
			modelBuilder.UseReferences();

			return modelBuilder;
		}

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
					Type valueType = idType.GetStronglyTypedIdValueType();

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

		/// <summary>
		///     Disables the delete cascading for references aggregate roots.
		/// </summary>
		/// <param name="modelBuilder"></param>
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
