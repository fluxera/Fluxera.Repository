namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
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
			await this.PersonRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			await this.PersonRepository.RemoveAsync(person);
			long count = await this.PersonRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemById()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			await this.PersonRepository.RemoveAsync(person.ID);
			long count = await this.PersonRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemByPredicate()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();

			await this.PersonRepository.RemoveRangeAsync(x => x.ID == person.ID);
			long count = await this.PersonRepository.CountAsync();
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
			await this.PersonRepository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			await this.PersonRepository.RemoveRangeAsync(persons);
			long count = await this.PersonRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemsByPredicate()
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
			await this.PersonRepository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			await this.PersonRepository.RemoveRangeAsync(x => true);
			long count = await this.PersonRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			await this.EmployeeRepository.RemoveAsync(employee);
			long count = await this.EmployeeRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemByIdWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			await this.EmployeeRepository.RemoveAsync(employee.ID);
			long count = await this.EmployeeRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemByPredicateWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			await this.EmployeeRepository.RemoveRangeAsync(x => x.ID == employee.ID);
			long count = await this.EmployeeRepository.CountAsync();
			count.Should().Be(0);
		}

		[Test]
		public async Task ShouldRemoveItemsWithStronglyTypedId()
		{
			Employee[] employees =
			{
				new Employee
				{
					Name = "Tester"
				},
				new Employee
				{
					Name = "Tester"
				},
				new Employee
				{
					Name = "Tester"
				},
				new Employee
				{
					Name = "Tester"
				}
			};
			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			await this.EmployeeRepository.RemoveRangeAsync(employees);
			long count = await this.EmployeeRepository.CountAsync();
			count.Should().Be(0);
		}
	}
}
