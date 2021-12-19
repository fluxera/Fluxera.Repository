namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class AddTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldAddItem()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();
		}

		[Test]
		public async Task ShouldAddItems()
		{
			Person[] persons =
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
			await this.Repository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());
		}
	}
}
