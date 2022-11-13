namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class ReferenceTests : ReferenceTestsBase
	{
		/// <inheritdoc />
		public ReferenceTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			repositoryBuilder.AddLiteRepository<RepositoryLiteContext>(repositoryName, configureOptions);
		}
	}
}
