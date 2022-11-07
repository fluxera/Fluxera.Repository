namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Repository.UnitTests.Core.ReferenceAggregate;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class ReferenceTestsBase : RepositoryTestBase
	{
		private Company company;
		private Employee employee;
		private Person person;

		/// <inheritdoc />
		protected override async Task OnSetUpAsync()
		{
			this.company = new Company
			{
				Name = "TestCompany",
				LegalType = LegalType.LimitedLiabilityCompany
			};

			this.person = new Person
			{
				Name = "TestPerson"
			};

			this.employee = new Employee
			{
				Name = "TestEmployee"
			};

			await this.CompanyRepository.AddAsync(this.company);
			await this.PersonRepository.AddAsync(this.person);
			await this.EmployeeRepository.AddAsync(this.employee);
			await this.UnitOfWork.SaveChangesAsync();
		}

		[Test]
		public async Task ShouldHaveInternalReference_WithStringId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				Company = this.company
			};
			await this.ReferenceRepository.AddAsync(reference);
			await this.UnitOfWork.SaveChangesAsync();

			reference = await this.ReferenceRepository.GetAsync(reference.ID);

			// Assert
			reference.Company.ID.Should().Be(this.company.ID);
			//reference.Company.Name.Should().BeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldHaveInternalReference_WithGuidId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				Person = this.person
			};
			await this.ReferenceRepository.AddAsync(reference);
			await this.UnitOfWork.SaveChangesAsync();

			reference = await this.ReferenceRepository.GetAsync(reference.ID);

			// Assert
			reference.Person.ID.Should().Be(this.person.ID);
			//reference.Person.Name.Should().BeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldHaveInternalReference_WithStronglyTypedId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				Employee = this.employee
			};
			await this.ReferenceRepository.AddAsync(reference);
			await this.UnitOfWork.SaveChangesAsync();

			reference = await this.ReferenceRepository.GetAsync(reference.ID);

			// Assert
			reference.Employee.ID.Should().Be(this.employee.ID);
			//reference.Employee.Name.Should().BeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldHaveMultipleInternalReference_WithStringId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				Companies =
				{
					this.company
				}
			};
			await this.ReferenceRepository.AddAsync(reference);
			await this.UnitOfWork.SaveChangesAsync();

			reference = await this.ReferenceRepository.GetAsync(reference.ID);

			// Assert
			reference.Companies[0].ID.Should().Be(this.company.ID);
			//reference.Companies[0].Name.Should().BeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldHaveMultipleInternalReference_WithGuidId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				People =
				{
					this.person
				}
			};
			await this.ReferenceRepository.AddAsync(reference);
			await this.UnitOfWork.SaveChangesAsync();

			reference = await this.ReferenceRepository.GetAsync(reference.ID);

			// Assert
			reference.People[0].ID.Should().Be(this.person.ID);
			//reference.People[0].Name.Should().BeNullOrWhiteSpace();
		}

		[Test]
		public async Task ShouldHaveMultipleInternalReference_StronglyTypedId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				Employees =
				{
					this.employee
				}
			};
			await this.ReferenceRepository.AddAsync(reference);
			await this.UnitOfWork.SaveChangesAsync();

			reference = await this.ReferenceRepository.GetAsync(reference.ID);

			// Assert
			reference.Employees[0].ID.Should().Be(this.employee.ID);
			//reference.Employees[0].Name.Should().BeNullOrWhiteSpace();
		}
	}
}
