namespace Fluxera.Repository.UnitTests.InMemory
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.Storage.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class AddTests : TestBase
	{
		[Test]
		public void ShouldAddValidItem()
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

			IRepository<Person> repository = serviceProvider.GetRequiredService<IRepository<Person>>();
		}
	}
}
