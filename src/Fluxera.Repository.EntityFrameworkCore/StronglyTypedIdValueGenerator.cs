namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using Fluxera.StronglyTypedId;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Microsoft.EntityFrameworkCore.ValueGeneration;

	/// <inheritdoc />
	[PublicAPI]
	public class StronglyTypedIdValueGenerator<TStronglyTypedId, TValue> : ValueGenerator<TStronglyTypedId>
		where TStronglyTypedId : StronglyTypedId<TStronglyTypedId, TValue>
		where TValue : IComparable, IComparable<TValue>, IEquatable<TValue>
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
		public override TStronglyTypedId Next(EntityEntry entry)
		{
			object result;

			if(typeof(TValue) == typeof(string))
			{
				Guid guid = this.sequentialGuidValueGenerator.Next(entry);
				result = Activator.CreateInstance(typeof(TStronglyTypedId), [guid.ToString("D")]);
			}
			else if(typeof(TValue) == typeof(Guid))
			{
				Guid guid = this.sequentialGuidValueGenerator.Next(entry);
				result = Activator.CreateInstance(typeof(TStronglyTypedId), [guid]);
			}
			else
			{
				throw new InvalidOperationException("The EFCore repository only supports guid or string as type for strongly-typed keys.");
			}

			return (TStronglyTypedId)result;
		}
	}
}
