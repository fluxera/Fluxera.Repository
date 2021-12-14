namespace Fluxera.Repository.UnitTests.PersonAggregate
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Person : AggregateRoot<Person>
	{
		public static readonly Person Valid = new Person
		{
			Name = "Tester"
		};

		public static readonly Person Invalid = new Person();

		public static readonly Person Null = null;

		public static readonly Person NotTransient = new Person
		{
			ID = Guid.NewGuid().ToString(),
			Name = "Tester"
		};

		public static readonly Person Transient = Valid;

		[Required]
		public string Name { get; set; }
	}
}
