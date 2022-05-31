namespace Fluxera.Repository.MongoDB
{
	using System;
	using Fluxera.StronglyTypedId;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.IdGenerators;

	/// <summary>
	///     Represents an Id generator for strongly-typed IDs.
	/// </summary>
	public class StronglyTypedIdGenerator : IIdGenerator
	{
		private readonly Type idType;

		/// <summary>
		///     Initializes a new instance of the <see cref="StronglyTypedIdGenerator" /> type.
		/// </summary>
		/// <param name="idType"></param>
		public StronglyTypedIdGenerator(Type idType)
		{
			this.idType = idType;
		}

		/// <inheritdoc />
		public object GenerateId(object container, object document)
		{
			object value = null;

			if(this.idType.IsStronglyTypedId())
			{
				Type valueType = this.idType.GetValueType();

				if(valueType == typeof(string))
				{
					value = StringObjectIdGenerator.Instance.GenerateId(container, document);
				}
				else if(valueType == typeof(Guid))
				{
					value = CombGuidGenerator.Instance.GenerateId(container, document);
				}
				else
				{
					throw new InvalidOperationException("The MongoDB repository only supports guid or string as type for keys.");
				}
			}

			object id = Activator.CreateInstance(this.idType, new object[] { value });
			return id;
		}

		/// <inheritdoc />
		public bool IsEmpty(object id)
		{
			return id == null;
		}
	}
}
