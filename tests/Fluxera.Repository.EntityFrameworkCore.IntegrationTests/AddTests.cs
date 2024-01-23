namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using System;
	using Fluxera.Repository.UnitTests.Core;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class AddTests : AddTestBase
	{
		/// <inheritdoc />
		public AddTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.Services.AddDbContext<RepositoryDbContext>();

			repositoryBuilder.AddEntityFrameworkRepository<RepositoryContext>(repositoryName, configureOptions.Invoke);
		}

		// TODO: Add Invoice/InvoiceItem tests here to check if referenced entites are add/updated
		//[Test]
		//public async Task ShouldAddEntity()
		//{
		//	Person person = new Person
		//	{
		//		Name = "Tester"
		//	};
		//	await this.PersonRepository.AddAsync(person);
		//	await this.UnitOfWork.SaveChangesAsync();

		//	person.ID.Should().NotBeEmpty();

		//	Person result = await this.PersonRepository.GetAsync(person.ID);
		//	result.Should().NotBeNull();
		//}
	}
}
