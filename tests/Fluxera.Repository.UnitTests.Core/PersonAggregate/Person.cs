namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System;
	using Fluxera.Entity;

	public class Person : Entity<Person, Guid>
	{
		public Person()
		{
			this.Address = new Address();
		}

		public string Name { get; set; }

		public int Age { get; set; }

		public Address Address { get; set; }
	}
}
