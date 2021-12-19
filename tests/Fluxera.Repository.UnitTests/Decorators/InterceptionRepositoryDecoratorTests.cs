namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class InterceptionRepositoryDecoratorTests : DecoratorTestBase
	{
		/// <inheritdoc />
		protected override Type DecoratorType => typeof(InterceptionRepositoryDecorator<,>);

		/// <inheritdoc />
		protected override Type RepositoryType => typeof(NoopTestRepository<Person, Guid>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(typeof(IDecoratingInterceptorFactory<,>), typeof(TestDecoratingInterceptorFactory<,>));
		}
	}
}
