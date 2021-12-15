namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Decorators;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class CachingRepositoryDecoratorTests : DecoratorTestBase
	{
		/// <inheritdoc />
		protected override Type DecoratorType => typeof(CachingRepositoryDecorator<>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<IRepositoryRegistry, TestRepositoryRegistry>();
			services.AddTransient<ICachingStrategyFactory, TestCachingStrategyFactory>();
		}

		[Test]
		public void Should()
		{
		}
	}
}
