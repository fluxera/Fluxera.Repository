namespace Fluxera.Repository.UnitTests.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class SortingTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldSortByPrimary()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Julie",
					Age = 23,
				},
				new Person
				{
					Name = "Hugo",
					Age = 32,
				},
				new Person
				{
					Name = "Zara",
					Age = 43,
				},
				new Person
				{
					Name = "Arnold",
					Age = 27,
				},
				new Person
				{
					Name = "Erica",
					Age = 32,
				},
				new Person
				{
					Name = "Peter",
					Age = 52,
				},
			};
			await this.Repository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.OrderBy(x => x.Name);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => x.Age < 35, options);
			IList<Person> resultList = new List<Person>(result);

			resultList.Count.Should().Be(4);
			resultList[0].Name.Should().Be("Arnold");
			resultList[1].Name.Should().Be("Erica");
			resultList[2].Name.Should().Be("Hugo");
			resultList[3].Name.Should().Be("Julie");
		}

		[Test]
		public virtual async Task ShouldSortByPrimaryAndSecondary()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Julie",
					Age = 27,
				},
				new Person
				{
					Name = "Arnold",
					Age = 32,
				},
				new Person
				{
					Name = "Julie",
					Age = 23,
				},
				new Person
				{
					Name = "Arnold",
					Age = 29,
				},
				new Person
				{
					Name = "Peter",
					Age = 52,
				},
			};
			await this.Repository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.OrderBy(x => x.Name).ThenBy(x => x.Age);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => x.Age < 35, options);
			IList<Person> resultList = new List<Person>(result);

			IOrderedEnumerable<Person> orderedEnumerable = persons.OrderBy(x => x.Name).ThenBy(x => x.Age);

			resultList.Count.Should().Be(4);
			resultList[0].Name.Should().Be("Arnold");
			resultList[0].Age.Should().Be(29);

			resultList[1].Name.Should().Be("Arnold");
			resultList[1].Age.Should().Be(32);

			resultList[2].Name.Should().Be("Julie");
			resultList[2].Age.Should().Be(23);

			resultList[3].Name.Should().Be("Julie");
			resultList[3].Age.Should().Be(27);
		}

		[Test]
		public async Task ShouldSortByDescendingPrimary()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Julie",
					Age = 23,
				},
				new Person
				{
					Name = "Hugo",
					Age = 32,
				},
				new Person
				{
					Name = "Zara",
					Age = 43,
				},
				new Person
				{
					Name = "Arnold",
					Age = 27,
				},
				new Person
				{
					Name = "Erica",
					Age = 32,
				},
				new Person
				{
					Name = "Peter",
					Age = 52,
				},
			};
			await this.Repository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.OrderByDescending(x => x.Name);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => x.Age < 35, options);
			IList<Person> resultList = new List<Person>(result);

			resultList.Count.Should().Be(4);
			resultList[3].Name.Should().Be("Arnold");
			resultList[2].Name.Should().Be("Erica");
			resultList[1].Name.Should().Be("Hugo");
			resultList[0].Name.Should().Be("Julie");
		}

		[Test]
		public virtual async Task ShouldSortByPrimaryAndSecondaryDescending()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Julie",
					Age = 27,
				},
				new Person
				{
					Name = "Arnold",
					Age = 32,
				},
				new Person
				{
					Name = "Julie",
					Age = 23,
				},
				new Person
				{
					Name = "Arnold",
					Age = 29,
				},
				new Person
				{
					Name = "Peter",
					Age = 52,
				},
			};
			await this.Repository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.OrderByDescending(x => x.Name).ThenByDescending(x => x.Age);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => x.Age < 35, options);
			IList<Person> resultList = new List<Person>(result);

			IOrderedEnumerable<Person> orderedEnumerable = persons.OrderBy(x => x.Name).ThenBy(x => x.Age);

			resultList.Count.Should().Be(4);
			resultList[3].Name.Should().Be("Arnold");
			resultList[3].Age.Should().Be(29);

			resultList[2].Name.Should().Be("Arnold");
			resultList[2].Age.Should().Be(32);

			resultList[1].Name.Should().Be("Julie");
			resultList[1].Age.Should().Be(23);

			resultList[0].Name.Should().Be("Julie");
			resultList[0].Age.Should().Be(27);
		}
	}
}
