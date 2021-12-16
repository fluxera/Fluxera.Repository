//namespace Fluxera.Repository.MongoDB.IntegrationTests
//{
//	using System;
//	using Fluxera.Repository.UnitTests.Core;
//	using NUnit.Framework;

//	[TestFixture]
//	public class AddTests : AddTestBase
//	{
//		/// <inheritdoc />
//		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
//			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
//		{
//			repositoryBuilder.AddMongoRepository(repositoryName, options =>
//			{
//				options.AddSetting("Mongo.ConnectionString", "mongodb://localhost:27017");
//				options.AddSetting("Mongo.Database", "test");

//				configureOptions.Invoke(options);
//			});
//		}
//	}
//}


