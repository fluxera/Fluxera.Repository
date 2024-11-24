namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.CompanyAggregate;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Repository.UnitTests.Core.ReferenceAggregate;
	using global::LiteDB;
	using NUnit.Framework;

	[TestFixture(true, true)]
	[TestFixture(true, false)]
	[TestFixture(false, true)]
	[TestFixture(false, false)]
	public class AggregateTests : AggregateTestBase
	{
		private readonly bool isPersistent;

		/// <inheritdoc />
		public AggregateTests(bool isUnitOfWorkEnabled, bool isPersistent)
			: base(isUnitOfWorkEnabled)
		{
			this.isPersistent = isPersistent;
		}

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

			if(this.isPersistent)
			{
				repositoryBuilder.AddLiteRepository<RepositoryLiteContext>(repositoryName, configureOptions);
			}
			else
			{
				repositoryBuilder.AddLiteRepository<RepositoryLiteContextInMemory>(repositoryName, configureOptions);
			}
		}
	}
}
