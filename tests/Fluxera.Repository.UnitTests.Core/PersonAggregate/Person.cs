namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Person : AggregateRoot<Person, Guid>
	{
		public Person()
		{
			this.Address = new Address();
		}

		[Required]
		public string Name { get; set; }

		public int Age { get; set; }

		public Address Address { get; set; }
	}
}
