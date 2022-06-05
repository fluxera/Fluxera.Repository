namespace Fluxera.Repository.UnitTests.Core.ReferenceAggregate
{
	using System;
	using System.Collections.Generic;
	using Fluxera.ComponentModel.Annotations;
	using Fluxera.Entity;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class Reference : AggregateRoot<Reference, string>
	{
		// References to external aggregate roots.

		[Reference("Company")]
		public string CompanyId { get; set; }

		[Reference("Person")]
		public Guid? PersonId { get; set; }

		[Reference("Employee")]
		public EmployeeId EmployeeId { get; set; }

		[Reference("Company")]
		public IList<string> CompanyIds { get; set; } = new List<string>();

		[Reference("Person")]
		public IList<Guid> PersonIds { get; set; } = new List<Guid>();

		[Reference("Employee")]
		public IList<EmployeeId> EmployeeIds { get; set; } = new List<EmployeeId>();

		// References to internal aggregate roots.

		[Reference(typeof(Company))]
		public Company Company { get; set; }

		//[Reference(typeof(Person))]
		public Person Person { get; set; }

		[Reference(typeof(Employee))]
		public Employee Employee { get; set; }

		[Reference(typeof(Company))]
		public IList<Company> Companies { get; set; } = new List<Company>();

		[Reference(typeof(Person))]
		public IList<Person> People { get; set; } = new List<Person>();

		[Reference(typeof(Employee))]
		public IList<Employee> Employees { get; set; } = new List<Employee>();

		//// References to external entities.

		//[Reference("ReferenceStringEntity")]
		//public string ReferenceStringEntityId { get; set; }

		//[Reference("ReferenceGuidEntity")]
		//public Guid? ReferenceGuidEntityId { get; set; }

		//[Reference("ReferenceStronglyTypedEntity")]
		//public ReferenceStronglyTypedEntityId ReferenceStronglyTypedEntityId { get; set; }

		//[Reference("ReferenceStringEntity")]
		//public IList<string> ReferenceStringEntityIds { get; set; }

		//[Reference("ReferenceGuidEntity")]
		//public IList<Guid> ReferenceGuidEntityIds { get; set; }

		//[Reference("ReferenceStronglyTypedEntity")]
		//public IList<ReferenceStronglyTypedEntityId> ReferenceStronglyTypedEntityIds { get; set; }

		//// References to internal entities.

		//[Reference(typeof(ReferenceStringEntity))]
		//public ReferenceStringEntity ReferenceStringEntity { get; set; }

		//[Reference(typeof(ReferenceGuidEntity))]
		//public ReferenceGuidEntity ReferenceGuidEntity { get; set; }

		//[Reference(typeof(ReferenceStronglyTypedEntity))]
		//public ReferenceStronglyTypedEntity ReferenceStronglyTypedEntity { get; set; }

		//[Reference(typeof(ReferenceStringEntity))]
		//public IList<ReferenceStringEntity> ReferenceStringEntites { get; set; }

		//[Reference(typeof(ReferenceGuidEntity))]
		//public IList<ReferenceGuidEntity> ReferenceGuidEntities { get; set; }

		//[Reference(typeof(ReferenceStronglyTypedEntity))]
		//public IList<ReferenceStronglyTypedEntity> ReferenceStronglyTypedEntities { get; set; }
	}
}
