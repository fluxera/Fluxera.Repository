namespace Fluxera.Repository.EntityFrameworkCore
{
	using Fluxera.Enumeration.EntityFrameworkCore;
	using Fluxera.StronglyTypedId.EntityFrameworkCore;
	using Fluxera.ValueObject.EntityFrameworkCore;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	///     Contains <see cref="DbContext" /> related extension methods.
	/// </summary>
	[PublicAPI]
	public static class DbContextExtensions
	{
		/// <summary>
		///     Configure the model builder.
		/// </summary>
		/// <param name="modelBuilder"></param>
		/// <returns></returns>
		public static ModelBuilder UseRepositoryDefaults(this ModelBuilder modelBuilder)
		{
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
	}
}
