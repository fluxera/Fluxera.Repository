namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
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

			Person item = await this.PersonRepository.GetAsync(person.ID);
			item.Name.Should().Be("Tester");
			item.Name = "John";
			await this.PersonRepository.UpdateAsync(item);

			Person result = await this.PersonRepository.GetAsync(person.ID);
			result.Name.Should().Be("John");
		}

		[Test]
		public async Task ShouldUpdateItemWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			Employee item = await this.EmployeeRepository.GetAsync(employee.ID);
			item.Name.Should().Be("Tester");
			item.Name = "John";
			await this.EmployeeRepository.UpdateAsync(item);

			Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
			result.Name.Should().Be("John");
		}

		[Test]
		public async Task ShouldUpdateItemsWithStronglyTypedId()
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
				}
			};
			await this.EmployeeRepository.AddRangeAsync(employees);

			employees[0].Name = "One";
			employees[1].Name = "Two";
			await this.EmployeeRepository.UpdateRangeAsync(employees);

			Employee result1 = await this.EmployeeRepository.GetAsync(employees[0].ID);
			result1.Name.Should().Be("One");

			Employee result2 = await this.EmployeeRepository.GetAsync(employees[1].ID);
			result2.Name.Should().Be("Two");
		}

		[Test]
		public async Task ShouldUpdateItems()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				},
				new Person
				{
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				}
			};
			await this.PersonRepository.AddRangeAsync(persons);

			persons[0].Name = "One";
			persons[1].Name = "Two";
			await this.PersonRepository.UpdateRangeAsync(persons);

			Person result1 = await this.PersonRepository.GetAsync(persons[0].ID);
			result1.Name.Should().Be("One");

			Person result2 = await this.PersonRepository.GetAsync(persons[1].ID);
			result2.Name.Should().Be("Two");
		}
	}
}
