//namespace Fluxera.Repository.OData.IntegrationTests
//{
//	using System;
//	using Fluxera.Repository.UnitTests.Core;
//	using NUnit.Framework;

//	[TestFixture]
//	public class RemoveTests : RemoveTestBase
//	{
//		/// <inheritdoc />
//		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
//			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
//		{
//			repositoryBuilder.AddODataRepository(repositoryName, options =>
//			{
//				options.AddSetting("OData.ServiceRoot", "https://localhost:5001/");
//				options.AddSetting("OData.UseBatching", true);

//				configureOptions.Invoke(options);
//			});
//		}
//	}
//}


