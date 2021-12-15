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
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeNullOrWhiteSpace();

			Person fromStore = await this.Repository.GetAsync(person.ID);
			fromStore.Name.Should().Be("Tester");
			fromStore.Name = "John";
			await this.Repository.UpdateAsync(fromStore);

			Person result = await this.Repository.GetAsync(fromStore.ID);
			result.Name.Should().Be("John");
		}

		[Test]
		public async Task ShouldUpdateItems()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeNullOrWhiteSpace();

			Person item = await this.Repository.GetAsync(person.ID);
			item.Name.Should().Be("Tester");
			item.Name = "John";
			await this.Repository.UpdateAsync(item);

			Person result = await this.Repository.GetAsync(person.ID);
			result.Name.Should().Be("John");
		}
	}
}
