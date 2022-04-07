namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class UpdateTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldUpdateItem()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			Person fromStore = await this.PersonRepository.GetAsync(person.ID);
			fromStore.Name.Should().Be("Tester");
			fromStore.Name = "John";
			await this.PersonRepository.UpdateAsync(fromStore);

			Person result = await this.PersonRepository.GetAsync(fromStore.ID);
			result.Name.Should().Be("John");
		}

		[Test]
		public async Task ShouldUpdateItems()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			Person item = await this.PersonRepository.GetAsync(person.ID);
			item.Name.Should().Be("Tester");
			item.Name = "John";
			await this.PersonRepository.UpdateAsync(item);

			Person result = await this.PersonRepository.GetAsync(person.ID);
			result.Name.Should().Be("John");
		}
	}
}
