namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.Specifications;
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
			services.AddSingleton<IRepositoryRegistry, TestRepositoryRegistry>();

			services.AddSingleton(typeof(IDecoratingInterceptorFactory<,>), typeof(TestDecoratingInterceptorFactory<,>));
		}

		private void ShouldHaveUsedInterceptor(Func<TestDecoratingInterceptor<Person, Guid>, bool> flagProviderFunc)
		{
			TestDecoratingInterceptorFactory<Person, Guid> decoratingInterceptorFactory = (TestDecoratingInterceptorFactory<Person, Guid>)this.ServiceProvider.GetRequiredService<IDecoratingInterceptorFactory<Person, Guid>>();
			TestDecoratingInterceptor<Person, Guid> decoratingInterceptor = (TestDecoratingInterceptor<Person, Guid>)decoratingInterceptorFactory.CreateDecoratingInterceptor();

			bool result = flagProviderFunc.Invoke(decoratingInterceptor);
			result.Should().BeTrue();
		}

		[Test]
		public async Task Should_Intercept_BeforeAdd()
		{
			await this.Repository.AddAsync(new Person
			{
				Name = "Tester"
			});

			this.ShouldHaveUsedInterceptor(x => x.BeforeAddCalled);
		}

		[Test]
		public async Task Should_Intercept_BeforeFind_Expression()
		{
			await this.Repository.FindManyAsync(x => true);

			this.ShouldHaveUsedInterceptor(x => x.BeforeFindExpressionCalled);
		}

		[Test]
		public async Task Should_Intercept_BeforeFind_Spec()
		{
			ISpecification<Person> spec = new Specification<Person>(x => true);
			await this.Repository.FindManyAsync(spec);

			this.ShouldHaveUsedInterceptor(x => x.BeforeFindSpecCalled);
		}

		[Test]
		public async Task Should_Intercept_BeforeRemove()
		{
			await this.Repository.RemoveAsync(new Person
			{
				Name = "Tester"
			});

			this.ShouldHaveUsedInterceptor(x => x.BeforeRemoveCalled);
		}

		[Test]
		public async Task Should_Intercept_BeforeRemoveRange_Expression()
		{
			await this.Repository.RemoveRangeAsync(x => true);

			this.ShouldHaveUsedInterceptor(x => x.BeforeRemoveRangeExpressionCalled);
		}

		[Test]
		public async Task Should_Intercept_BeforeRemoveRange_Spec()
		{
			ISpecification<Person> spec = new Specification<Person>(x => true);
			await this.Repository.RemoveRangeAsync(spec);

			this.ShouldHaveUsedInterceptor(x => x.BeforeRemoveRangeSpecCalled);
		}

		[Test]
		public async Task Should_Intercept_BeforeUpdate()
		{
			await this.Repository.UpdateAsync(new Person
			{
				Name = "Tester"
			});

			this.ShouldHaveUsedInterceptor(x => x.BeforeUpdateCalled);
		}
	}
}
