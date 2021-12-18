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
	public abstract class PagingTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldPageResult()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.Paging(5, 10);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => x.Age < 40, options);

			result.Count.Should().Be(10);
			result.All(x => x.Age < 40).Should().BeTrue();
		}

		[Test]
		public async Task ShouldSkip()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.Skip(100);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => true, options);

			result.Count.Should().Be(150);
		}

		[Test]
		public async Task ShouldTake()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.Take(75);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => true, options);

			result.Count.Should().Be(75);
		}

		[Test]
		public async Task ShouldSkipTake()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.SkipTake(200, 75);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => true, options);

			result.Count.Should().Be(50);
		}

		[Test]
		public async Task ShouldSkipTakeFluent()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			IQueryOptions<Person> options = QueryOptions<Person>.Skip(200).Take(75);
			IReadOnlyCollection<Person> result = await this.Repository.FindManyAsync(x => true, options);

			result.Count.Should().Be(50);
		}
	}
}
