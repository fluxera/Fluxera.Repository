namespace WebApplication1
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Person : AggregateRoot<Person, Guid>
	{
		[Required]
		public string Name { get; set; }

		public int Age { get; set; }
	}
}
