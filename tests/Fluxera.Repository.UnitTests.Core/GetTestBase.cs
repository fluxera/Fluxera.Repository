namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class GetTestBase : RepositoryTestBase
	{
		/// <inheritdoc />
		protected GetTestBase(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		[Test]
		public async Task ShouldGetById()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			await this.UnitOfWork.SaveChangesAsync();

			person.ID.Should().NotBeEmpty();

			Person fromStore = await this.PersonRepository.GetAsync(person.ID);
			fromStore.Should().NotBeNull();
			fromStore.ID.Should().Be(person.ID);
		}

		[Test]
		public async Task ShouldGetByIdWithSelector()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			await this.UnitOfWork.SaveChangesAsync();

			person.ID.Should().NotBeEmpty();

			string fromStore = await this.PersonRepository.GetAsync(person.ID, x => x.Name);
			fromStore.Should().NotBeNull();
			fromStore.Should().Be(person.Name);
		}

		[Test]
		public async Task ShouldExistsById()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			await this.PersonRepository.AddAsync(person);
			await this.UnitOfWork.SaveChangesAsync();

			person.ID.Should().NotBeEmpty();

			bool fromStore = await this.PersonRepository.ExistsAsync(person.ID);
			fromStore.Should().BeTrue();
		}

		[Test]
		public async Task ShouldGetByIdWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			Employee fromStore = await this.EmployeeRepository.GetAsync(employee.ID);
			fromStore.Should().NotBeNull();
			fromStore.ID.Should().Be(employee.ID);
		}

		[Test]
		public async Task ShouldGetByIdWithStronglyTypedId_NewIdCopy()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			EmployeeId newId = new EmployeeId(employee.ID.Value);
			Employee fromStore = await this.EmployeeRepository.GetAsync(newId);

			fromStore.Should().NotBeNull();
			fromStore.ID.Should().Be(employee.ID);
		}

		[Test]
		public async Task ShouldGetByIdWithSelectorWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			string fromStore = await this.EmployeeRepository.GetAsync(employee.ID, x => x.Name);
			fromStore.Should().NotBeNull();
			fromStore.Should().Be(employee.Name);
		}

		[Test]
		public async Task ShouldExistsByIdWithStronglyTypedId()
		{
			Employee employee = new Employee
			{
				Name = "Tester"
			};
			await this.EmployeeRepository.AddAsync(employee);
			await this.UnitOfWork.SaveChangesAsync();

			employee.ID.Should().NotBeNull();
			employee.ID.Value.Should().NotBeEmpty();

			bool fromStore = await this.EmployeeRepository.ExistsAsync(employee.ID);
			fromStore.Should().BeTrue();
		}
	}
}
