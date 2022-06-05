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
		}

		[Test]
		public async Task ShouldHaveExternalReference_WithStringId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				CompanyId = this.company.ID
			};
			await this.ReferenceRepository.AddAsync(reference);

			// Assert
			reference.CompanyId.Should().Be(this.company.ID);
		}

		[Test]
		public async Task ShouldHaveExternalReference_WithGuidId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				PersonId = this.person.ID
			};
			await this.ReferenceRepository.AddAsync(reference);

			// Assert
			reference.PersonId.Should().Be(this.person.ID);
		}

		[Test]
		public async Task ShouldHaveExternalReference_WithStronglyTypedId()
		{
			// Arrange & Act
			Reference reference = new Reference
			{
				EmployeeId = this.employee.ID
			};
			await this.ReferenceRepository.AddAsync(reference);

			// Assert
			reference.EmployeeId.Should().Be(this.employee.ID);
		}
	}
}
