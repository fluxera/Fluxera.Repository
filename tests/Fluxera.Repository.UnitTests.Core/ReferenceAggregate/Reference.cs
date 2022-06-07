namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	using System.Collections.Generic;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class Reference : AggregateRoot<Reference, string>
	{
		// References to aggregate roots.

		[Reference(typeof(Company))]
		public Company Company { get; set; }

		[Reference(typeof(Person))]
		public Person Person { get; set; }

		[Reference(typeof(Employee))]
		public Employee Employee { get; set; }

		[Reference(typeof(Company))]
		public IList<Company> Companies { get; set; } = new List<Company>();

		[Reference(typeof(Person))]
		public IList<Person> People { get; set; } = new List<Person>();

		[Reference(typeof(Employee))]
		public IList<Employee> Employees { get; set; } = new List<Employee>();
	}
}
