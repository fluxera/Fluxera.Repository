namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Repository.UnitTests.Core.ReferenceAggregate;
	using global::LiteDB;
	using NUnit.Framework;

	[TestFixture]
	public class SortingTests : SortingTestBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			BsonMapper.Global.Entity<Company>().Id(x => x.ID);
			BsonMapper.Global.Entity<Person>().Id(x => x.ID);
			BsonMapper.Global.Entity<Employee>().Id(x => x.ID);
			BsonMapper.Global.Entity<Reference>().Id(x => x.ID);

			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			repositoryBuilder.AddLiteRepository(repositoryName, options =>
			{
				options.AddSetting("Lite.Database", $"{Guid.NewGuid():N}.db");

				configureOptions.Invoke(options);
			});
		}

		[Test]
		public override async Task ShouldSortByPrimaryAndSecondary()
		{
			Func<Task> func = async () => await base.ShouldSortByPrimaryAndSecondary();
			await func.Should().ThrowAsync<NotSupportedException>();
		}

		[Test]
		public override async Task ShouldSortByPrimaryAndSecondaryDescending()
		{
			Func<Task> func = async () => await base.ShouldSortByPrimaryAndSecondaryDescending();
			await func.Should().ThrowAsync<NotSupportedException>();
		}
	}
}
