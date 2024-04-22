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
		/// <inheritdoc />
		protected PagingTestBase(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		[Test]
		public async Task ShouldPageResult()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());


/* Unmerged change from project 'Fluxera.Repository.UnitTests.Core (net7.0)'
Before:
			IQueryOptions<Person> options = this.CreateQueryOptionsBuilder<Person>().Paging(5, 10).Build();
After:
			IQueryOptions<Person> options = CreateQueryOptionsBuilder<Person>().Paging(5, 10).Build();
*/
			IQueryOptions<Person> options = RepositoryTestBase.CreateQueryOptionsBuilder<Person>().Paging(5, 10).Build();
			IReadOnlyCollection<Person> result = await this.PersonRepository.FindManyAsync(x => x.Age < 40, options);

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

			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());


/* Unmerged change from project 'Fluxera.Repository.UnitTests.Core (net7.0)'
Before:
			IQueryOptions<Person> options = this.CreateQueryOptionsBuilder<Person>().Skip(100).Build();
After:
			IQueryOptions<Person> options = CreateQueryOptionsBuilder<Person>().Skip(100).Build();
*/
			IQueryOptions<Person> options = RepositoryTestBase.CreateQueryOptionsBuilder<Person>().Skip(100).Build();
			IReadOnlyCollection<Person> result = await this.PersonRepository.FindManyAsync(x => true, options);

			result.Count.Should().Be(150);
		}

		[Test]
		public async Task ShouldTake()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());


/* Unmerged change from project 'Fluxera.Repository.UnitTests.Core (net7.0)'
Before:
			IQueryOptions<Person> options = this.CreateQueryOptionsBuilder<Person>().Take(75).Build();
After:
			IQueryOptions<Person> options = CreateQueryOptionsBuilder<Person>().Take(75).Build();
*/
			IQueryOptions<Person> options = RepositoryTestBase.CreateQueryOptionsBuilder<Person>().Take(75).Build();
			IReadOnlyCollection<Person> result = await this.PersonRepository.FindManyAsync(x => true, options);

			result.Count.Should().Be(75);
		}

		[Test]
		public async Task ShouldSkipTake()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());


/* Unmerged change from project 'Fluxera.Repository.UnitTests.Core (net7.0)'
Before:
			IQueryOptions<Person> options = this.CreateQueryOptionsBuilder<Person>().SkipTake(200, 75).Build();
After:
			IQueryOptions<Person> options = CreateQueryOptionsBuilder<Person>().SkipTake(200, 75).Build();
*/
			IQueryOptions<Person> options = RepositoryTestBase.CreateQueryOptionsBuilder<Person>().SkipTake(200, 75).Build();
			IReadOnlyCollection<Person> result = await this.PersonRepository.FindManyAsync(x => true, options);

			result.Count.Should().Be(50);
		}

		[Test]
		public async Task ShouldSkipTakeFluent()
		{
			IList<Person> persons = new Bogus.Faker<Person>()
				.RuleFor(x => x.Name, faker => faker.Person.FirstName)
				.RuleFor(x => x.Age, faker => faker.Person.DateOfBirth.CalculateAge())
				.Generate(250);

			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());


/* Unmerged change from project 'Fluxera.Repository.UnitTests.Core (net7.0)'
Before:
			IQueryOptions<Person> options = this.CreateQueryOptionsBuilder<Person>().Skip(200).Take(75).Build();
After:
			IQueryOptions<Person> options = CreateQueryOptionsBuilder<Person>().Skip(200).Take(75).Build();
*/
			IQueryOptions<Person> options = RepositoryTestBase.CreateQueryOptionsBuilder<Person>().Skip(200).Take(75).Build();
			IReadOnlyCollection<Person> result = await this.PersonRepository.FindManyAsync(x => true, options);

			result.Count.Should().Be(50);
		}
	}
}
