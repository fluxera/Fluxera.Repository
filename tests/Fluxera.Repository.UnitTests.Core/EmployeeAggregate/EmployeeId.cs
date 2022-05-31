namespace Fluxera.Repository.UnitTests.Core.EmployeeAggregate
{
	using Fluxera.StronglyTypedId;

	public class EmployeeId : StronglyTypedId<EmployeeId, string>
	{
		/// <inheritdoc />
		public EmployeeId(string value) : base(value)
		{
		}
	}
}
