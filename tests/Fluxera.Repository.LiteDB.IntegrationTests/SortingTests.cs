namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture]
	public class SortingTests : SortingTestBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			repositoryBuilder.AddLiteRepository(repositoryName, options =>
			{
				options.AddSetting("Lite.Database", "test.db");

				configureOptions.Invoke(options);
			});
		}

		[Test]
		public override async Task ShouldSortByPrimaryAndSecondary()
		{
			Func<Task> func = async () => await base.ShouldSortByPrimaryAndSecondary();
			func.Should().ThrowAsync<NotSupportedException>();
		}

		[Test]
		public override async Task ShouldSortByPrimaryAndSecondaryDescending()
		{
			Func<Task> func = async () => await base.ShouldSortByPrimaryAndSecondaryDescending();
			func.Should().ThrowAsync<NotSupportedException>();
		}
	}
}
