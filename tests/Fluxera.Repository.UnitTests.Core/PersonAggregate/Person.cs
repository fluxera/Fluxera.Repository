namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System.ComponentModel.DataAnnotations;
	using Fluxera.Entity;

	public class Person : AggregateRoot<Person, string>
	{
		[Required]
		public string Name { get; set; }
	}
}
