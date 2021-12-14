//namespace Fluxera.Repository.UnitTests
//{
//	using System;
//	using Fluxera.Extensions.Validation.DataAnnotations;
//	using Fluxera.Repository.Storage.InMemory;
//	using Fluxera.Repository.UnitTests.PersonAggregate;
//	using Microsoft.Extensions.DependencyInjection;
//	using NUnit.Framework;

//	[TestFixture]
//	public class InterceptionRepositoryDecoratorTests : TestBase
//	{
//		private IRepository<Person> repository;

//		[SetUp]
//		public void SetUp()
//		{
//			IServiceProvider serviceProvider = BuildServiceProvider(services =>
//			{
//				services.AddRepository(rb =>
//				{
//					rb.AddInMemoryRepository("InMemory", rob =>
//					{
//						rob.UseFor<Person>();

//						rob.AddValidation(vob =>
//						{
//							vob.AddValidatorFactory(vb =>
//							{
//								vb.AddDataAnnotations(vob.RepositoryName);
//							});
//						});
//					});
//				});
//			});

//			this.repository = serviceProvider.GetRequiredService<IRepository<Person>>();
//		}

//		[Test]
//		public void Should()
//		{
//			this.repository.Dispose();
//		}
//	}
//}
