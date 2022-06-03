namespace Fluxera.Repository.UnitTests.Core.EmployeeAggregate
{
	using Fluxera.Entity;

	public class Employee : AggregateRoot<Employee, EmployeeId>
	{
		public string Name { get; set; }

		public int SalaryInt { get; set; }

		public long SalaryLong { get; set; }

		public decimal SalaryDecimal { get; set; }

		public float SalaryFloat { get; set; }

		public double SalaryDouble { get; set; }

		public int? SalaryNullableInt { get; set; }

		public long? SalaryNullableLong { get; set; }

		public decimal? SalaryNullableDecimal { get; set; }

		public float? SalaryNullableFloat { get; set; }

		public double? SalaryNullableDouble { get; set; }
	}
}
