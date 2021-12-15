namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using NUnit.Framework;

	[TestFixture]
	public class CachingRepositoryDecoratorTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					rb.AddInMemoryRepository("InMemory", rob =>
					{
						rob.UseFor<Person>();

						rob.AddValidation(vob =>
						{
							vob.AddValidatorFactory(vb =>
							{
								vb.AddDataAnnotations(vob.RepositoryName);
							});
						});
					});
				});
			});

			this.repository = serviceProvider.GetRequiredService<IRepository<Person>>();
			this.logger = serviceProvider.GetRequiredService<ILogger<TestBase>>();
		}

		private IRepository<Person> repository;
		private ILogger<TestBase> logger;

		[Test]
		public void Should()
		{
		}
	}
}
