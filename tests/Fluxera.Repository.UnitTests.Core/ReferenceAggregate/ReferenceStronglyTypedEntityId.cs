namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	using Fluxera.StronglyTypedId;

	public sealed class ReferenceStronglyTypedEntityId : StronglyTypedId<ReferenceStronglyTypedEntityId, string>
	{
		/// <inheritdoc />
		public ReferenceStronglyTypedEntityId(string value) : base(value)
		{
		}
	}
}
