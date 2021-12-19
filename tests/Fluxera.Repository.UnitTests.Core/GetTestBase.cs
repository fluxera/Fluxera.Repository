namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class GetTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldGetById()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			Person fromStore = await this.Repository.GetAsync(person.ID);
			fromStore.Should().NotBeNull();
			fromStore.ID.Should().Be(person.ID);
		}

		[Test]
		public async Task ShouldGetByIdWithSelector()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			string fromStore = await this.Repository.GetAsync(person.ID, x => x.Name);
			fromStore.Should().NotBeNull();
			fromStore.Should().Be(person.Name);
		}

		[Test]
		public async Task ShouldExistsById()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			bool fromStore = await this.Repository.ExistsAsync(person.ID);
			fromStore.Should().BeTrue();
		}
	}
}
