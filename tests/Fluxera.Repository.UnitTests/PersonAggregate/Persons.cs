namespace Fluxera.Repository.UnitTests.PersonAggregate
{
	using System;

	public static class Persons
	{
		public static readonly Person[] Valid =
		{
			new Person
			{
				Name = "Tester"
			}
		};

		public static readonly Person[] Invalid =
		{
			new Person
			{
				Name = "Tester"
			},
			new Person()
		};

		public static readonly Person[] Null = null;

		public static readonly Person[] Empty = Array.Empty<Person>();

		public static readonly Person[] NotTransient =
		{
			new Person
			{
				ID = Guid.NewGuid().ToString(),
				Name = "Tester"
			},
			new Person
			{
				ID = Guid.NewGuid().ToString(),
				Name = "Tester"
			}
		};

		public static readonly Person[] Transient =
		{
			new Person
			{
				Name = "Tester"
			},
			new Person
			{
				Name = "Tester"
			}
		};
	}
}
