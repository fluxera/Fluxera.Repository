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
		/// <inheritdoc />
		protected RemoveTestBase(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		[Test]
		public async Task ShouldRemoveItem()
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

			await this.PersonRepository.RemoveAsync(person);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			person.ID.Should().NotBeEmpty();

			Person result = await this.PersonRepository.GetAsync(person.ID);
			result.Should().NotBeNull();

			await this.PersonRepository.RemoveAsync(person.ID);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			person.ID.Should().NotBeEmpty();

			Person result = await this.PersonRepository.GetAsync(person.ID);
			result.Should().NotBeNull();

			await this.PersonRepository.RemoveRangeAsync(x => x.ID == person.ID);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			foreach(Person person in persons)
			{
				Person result = await this.PersonRepository.GetAsync(person.ID);
				result.Should().NotBeNull();
			}

			await this.PersonRepository.RemoveRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			foreach(Person person in persons)
			{
				Person result = await this.PersonRepository.GetAsync(person.ID);
				result.Should().NotBeNull();
			}

			await this.PersonRepository.RemoveRangeAsync(x => true);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
			result.Should().NotBeNull();

			await this.EmployeeRepository.RemoveAsync(employee);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
			result.Should().NotBeNull();

			await this.EmployeeRepository.RemoveAsync(employee.ID);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
			result.Should().NotBeNull();

			await this.EmployeeRepository.RemoveRangeAsync(x => x.ID == employee.ID);
			await this.UnitOfWork.SaveChangesAsync();

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
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			foreach(Employee employee in employees)
			{
				Employee result = await this.EmployeeRepository.GetAsync(employee.ID);
				result.Should().NotBeNull();
			}

			await this.EmployeeRepository.RemoveRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			long count = await this.EmployeeRepository.CountAsync();
			count.Should().Be(0);
		}
	}
}
