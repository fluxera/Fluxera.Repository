namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class RemoveTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldRemoveItem()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			await this.Repository.RemoveAsync(person);
			long count = await this.Repository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemById()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			await this.Repository.RemoveAsync(person.ID);
			long count = await this.Repository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemByPredicate()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.Repository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			await this.Repository.RemoveRangeAsync(x => x.ID == person.ID);
			long count = await this.Repository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItems()
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
				},
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

			await this.Repository.RemoveRangeAsync(persons);
			long count = await this.Repository.CountAsync();
			count.Should().Be(0);
		}
	}
}
