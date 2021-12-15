namespace Fluxera.Repository.UnitTests
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using JetBrains.Annotations;
	using NUnit.Framework;
	using static ParamsUtil;

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

			person.ID.Should().NotBeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldAddItems()
		{
			Person person1 = new Person
			{
				Name = "Tester"
			};
			Person person2 = new Person
			{
				Name = "Tester"
			};
			Person person3 = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(AsEnumerable(person1, person2, person3));

			person1.ID.Should().NotBeNullOrWhiteSpace();
			person2.ID.Should().NotBeNullOrWhiteSpace();
			person3.ID.Should().NotBeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldUpdateItem()
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
