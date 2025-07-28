namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Repository.UnitTests.Core;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	public abstract class DecoratorTestBase : TestBase
	{
		protected IRepository<Person, Guid> Repository { get; private set; }

		protected abstract Type DecoratorType { get; }

		protected virtual Type RepositoryType => typeof(TestRepository<Person, Guid>);

		protected IServiceProvider ServiceProvider { get; private set; }

		[SetUp]
		public void SetUp()
		{
			this.ServiceProvider = BuildServiceProvider(
				services =>
				{
					services
						.AddTransient(typeof(IRepository<Person, Guid>), this.RepositoryType)
						.Decorate(typeof(IRepository<,>))
						.With(this.DecoratorType);

					this.ConfigureServices(services);
				});

			this.Repository = this.ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
		}

		protected virtual void ConfigureServices(IServiceCollection services)
		{
		}
	}
}
