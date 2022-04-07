namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class EnumerationTestsBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldStoreEnumeration()
		{
			Company company = new Company
			{
				Name = "Test LLC",
				LegalType = LegalType.LimitedLiabilityCompany,
			};
			await this.CompanyRepository.AddAsync(company);
			company.ID.Should().NotBeEmpty();

			Company result = await this.CompanyRepository.GetAsync(company.ID);
			result.LegalType.Should().Be(LegalType.LimitedLiabilityCompany);
		}
	}
}
