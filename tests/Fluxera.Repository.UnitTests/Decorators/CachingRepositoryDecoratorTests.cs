namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class CachingRepositoryDecoratorTests : DecoratorTestBase
	{
		/// <inheritdoc />
		protected override Type DecoratorType => typeof(CachingRepositoryDecorator<>);

		/// <inheritdoc />
		protected override Type RepositoryType => typeof(NoopTestRepository<Person>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IRepositoryRegistry, TestRepositoryRegistry>();
			services.AddSingleton<ICachingStrategyFactory, TestCachingStrategyFactory>();
			services.AddSingleton(sp => (TestCachingStrategyFactory)sp.GetRequiredService<ICachingStrategyFactory>());
		}

		private void ShouldHaveUsedStrategy(Func<TestCachingStrategy<Person>, bool> flagProviderFunc)
		{
			TestCachingStrategyFactory cachingStrategyFactory = this.ServiceProvider.GetRequiredService<TestCachingStrategyFactory>();
			TestCachingStrategy<Person> cachingStrategy = cachingStrategyFactory.GetStrategy<Person>();

			bool result = flagProviderFunc.Invoke(cachingStrategy);
			result.Should().BeTrue();
		}

		[Test]
		public async Task Should_AddAsync_Multiple()
		{
			await this.Repository.AddAsync(Persons.Transient);

			this.ShouldHaveUsedStrategy(x => x.AddMultipleWasCalled);
		}

		[Test]
		public async Task Should_AddAsync_Single()
		{
			await this.Repository.AddAsync(Person.Transient);

			this.ShouldHaveUsedStrategy(x => x.AddSingleWasCalled);
		}

		[Test]
		public async Task Should_CountAsync()
		{
		}

		[Test]
		public async Task Should_CountAsync_Predicate()
		{
		}

		[Test]
		public async Task Should_ExistsAsync_Predicate()
		{
		}

		[Test]
		public async Task Should_ExistsAsync_Single()
		{
		}

		[Test]
		public async Task Should_FindManyAsync_Predicate()
		{
		}

		[Test]
		public async Task Should_FindManyAsync_Result()
		{
		}

		[Test]
		public async Task Should_FindOneAsync_Predicate()
		{
		}

		[Test]
		public async Task Should_FindOneAsync_Result()
		{
		}

		[Test]
		public async Task Should_GetAsync_Single()
		{
		}

		[Test]
		public async Task Should_GetAsync_Single_Result()
		{
		}

		[Test]
		public async Task Should_RemoveAsync_Multiple_Predicate()
		{
		}

		[Test]
		public async Task Should_RemoveAsync_Single()
		{
		}

		[Test]
		public async Task Should_RemoveAsync_Single_Identifier()
		{
		}

		[Test]
		public async Task Should_UpdateAsync_Multiple()
		{
			await this.Repository.UpdateAsync(Persons.NotTransient);

			this.ShouldHaveUsedStrategy(x => x.UpdateMultipleWasCalled);
		}

		[Test]
		public async Task Should_UpdateAsync_Single()
		{
			await this.Repository.UpdateAsync(Person.NotTransient);

			this.ShouldHaveUsedStrategy(x => x.UpdateSingleWasCalled);
		}
	}
}
