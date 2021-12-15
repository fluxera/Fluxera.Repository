namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Repository.UnitTests.Decorators;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class DecoratorTestBase : TestBase
	{
		protected IRepository<Person> Repository { get; private set; }

		protected abstract Type DecoratorType { get; }

		[SetUp]
		public void SetUp()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(services =>
			{
				services
					.AddTransient<IRepository<Person>, TestRepository<Person>>()
					.Decorate(typeof(IRepository<>))
					.With(this.DecoratorType);

				this.ConfigureServices(services);
			});

			this.Repository = serviceProvider.GetRequiredService<IRepository<Person>>();
		}

		protected virtual void ConfigureServices(IServiceCollection services)
		{
		}
	}
}
