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
			Company company = new Company
			{
				Name = "Tester",
				LegalType = LegalType.LimitedLiabilityCompany
			};
			await this.CompanyRepository.AddAsync(company);
			await this.UnitOfWork.SaveChangesAsync();

			company.ID.Should().NotBeEmpty();

			Company result = await this.CompanyRepository.GetAsync(company.ID);
			result.Should().NotBeNull();
		}

		[Test]
		public async Task ShouldAddItem()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			await this.UnitOfWork.SaveChangesAsync();

			person.ID.Should().NotBeEmpty();

			Person result = await this.PersonRepository.GetAsync(person.ID);
			result.Should().NotBeNull();
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
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			foreach(Person person in persons)
			{
				Person result = await this.PersonRepository.GetAsync(person.ID);
				result.Should().NotBeNull();
			}
		}

		[Test]
		public async Task ShouldAddItemWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
			result.Should().NotBeNull();
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
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			foreach(Employee employee in employees)
			{
				Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
				result.Should().NotBeNull();
			}
		}
	}
}
