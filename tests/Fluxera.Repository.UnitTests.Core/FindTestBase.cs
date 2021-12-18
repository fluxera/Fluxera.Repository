namespace Fluxera.Repository.UnitTests.Core
{
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class FindTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldFindOne()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester1"
				},
				new Person
				{
					Name = "Tester2"
				},
				new Person
				{
					Name = "Tester3"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			Person fromStore = await this.Repository.FindOneAsync(x => x.Name.EndsWith("2"));
			fromStore.Should().NotBeNull();
			fromStore.Name.Should().Be(persons[1].Name);
		}

		[Test]
		public async Task ShouldFindOneWithSelector()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester1"
				},
				new Person
				{
					Name = "Tester2"
				},
				new Person
				{
					Name = "Tester3"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			string fromStore = await this.Repository.FindOneAsync(x => x.Name.EndsWith("2"), x => x.Name);
			fromStore.Should().NotBeNullOrWhiteSpace();
			fromStore.Should().Be(persons[1].Name);
		}

		[Test]
		public async Task ShouldExistsByPredicate()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester1"
				},
				new Person
				{
					Name = "Tester2"
				},
				new Person
				{
					Name = "Tester3"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			bool fromStore = await this.Repository.ExistsAsync(x => x.Name.EndsWith("2"));
			fromStore.Should().BeTrue();
		}

		[Test]
		public async Task ShouldFindManyWith()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			Person[] fromStore = (await this.Repository.FindManyAsync(x => x.Name.EndsWith("2"))).ToArray();
			fromStore.ForEach(x => x.ID.Should().NotBeEmpty());
			fromStore.ForEach(x => x.Name.Should().EndWith("2"));
		}

		[Test]
		public async Task ShouldFindManyWithSelector()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			string[] fromStore = (await this.Repository.FindManyAsync(x => x.Name.EndsWith("2"), x => x.Name)).ToArray();
			fromStore.ForEach(x => x.Should().NotBeNullOrWhiteSpace());
			fromStore.ForEach(x => x.Should().EndWith("2"));
		}
	}
}
