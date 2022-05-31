namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class AddTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldAddItemStringId()
		{
			Company person = new Company
			{
				Name = "Tester",
				LegalType = LegalType.LimitedLiabilityCompany
			};
			await this.CompanyRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();
		}

		[Test]
		public async Task ShouldAddItem()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			person.ID.Should().NotBeEmpty();
		}

		[Test]
		public async Task ShouldAddItems()
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
				}
			};
			await this.PersonRepository.AddRangeAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());
		}

		[Test]
		public async Task ShouldAddItemWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();
		}

		[Test]
		public async Task ShouldAddItemsStronglyTypedId()
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
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());
		}
	}
}
