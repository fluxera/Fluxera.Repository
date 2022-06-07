namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture]
	public class ReferenceTests : ReferenceTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
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
	}
}
