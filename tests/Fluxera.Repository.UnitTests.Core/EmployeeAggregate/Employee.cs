namespace Fluxera.Repository.UnitTests.Core.EmployeeAggregate
{
	using Fluxera.Entity;

	public class Employee : AggregateRoot<Employee, EmployeeId>
	{
		public string Name { get; set; }
	}
}
