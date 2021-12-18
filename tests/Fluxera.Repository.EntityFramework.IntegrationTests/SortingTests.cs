//namespace Fluxera.Repository.InMemory.IntegrationTests
//{
//	using System;
//	using Fluxera.Repository.EntityFramework;
//	using Fluxera.Repository.UnitTests.Core;
//	using NUnit.Framework;

//	[TestFixture]
//	public class SortingTests : SortingTestBase
//	{
//		/// <inheritdoc />
//		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
//			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
//		{
//			repositoryBuilder.AddEntityFrameworkRepository(repositoryName, builder =>
//			{
//				configureOptions.Invoke(builder);

//				builder.AddSetting("EntityFramework.DbContext", typeof(RepositoryDbContext));
//				builder.AddSetting("EntityFramework.ConnectionString", "Test");
//				builder.AddSetting("EntityFramework.LogSQL", false);
//			});
//		}
//	}
//}
