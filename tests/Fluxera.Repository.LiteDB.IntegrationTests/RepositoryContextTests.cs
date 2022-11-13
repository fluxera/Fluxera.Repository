namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using global::LiteDB;
	using NUnit.Framework;

	[TestFixture]
	public class RepositoryContextTests : RepositoryContextTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			BsonMapper.Global.Entity<Person>().Id(x => x.ID);

			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			repositoryBuilder.AddLiteRepository<RepositoryMultiTenantLiteContext>(repositoryName, configureOptions);
		}
	}
}
