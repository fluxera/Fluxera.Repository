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
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture(true)]
	[TestFixture(false)]
	public class GetTests : GetTestBase
	{
		/// <inheritdoc />
		public GetTests(bool isUnitOfWorkEnabled)
			: base(isUnitOfWorkEnabled)
		{
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

			repositoryBuilder.Services.AddLiteContext(serviceProvider =>
			{
				DatabaseProvider databaseProvider = serviceProvider.GetRequiredService<DatabaseProvider>();
				IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();

				return new RepositoryLiteContext(repositoryName, databaseProvider, repositoryRegistry);
			});

			repositoryBuilder.AddLiteRepository<RepositoryLiteContext>(repositoryName, options =>
			{
				options.AddSetting("Lite.Database", $"{Guid.NewGuid():N}.db");

				configureOptions.Invoke(options);
			});
		}
	}
}
