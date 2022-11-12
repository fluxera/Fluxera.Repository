namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Microsoft.EntityFrameworkCore.ValueGeneration;

	/// <inheritdoc />
	[PublicAPI]
	public class SequentialGuidStringValueGenerator : ValueGenerator<string>
	{
		private readonly SequentialGuidValueGenerator sequentialGuidValueGenerator = new SequentialGuidValueGenerator();

		/// <summary>
		///     Gets a value indicating whether the values generated are temporary or permanent. This implementation
		///     always returns false, meaning the generated values will be saved to the database.
		/// </summary>
		public override bool GeneratesTemporaryValues => false;

		/// <summary>
		///     Gets a value to be assigned to a property.
		/// </summary>
		/// <param name="entry">The change tracking entry of the entity for which the value is being generated.</param>
		/// <returns>The value to be assigned to a property.</returns>
		public override string Next(EntityEntry entry)
		{
			Guid guid = this.sequentialGuidValueGenerator.Next(entry);
			return guid.ToString("D");
		}
	}
}
