namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Caching;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class CachingRepositoryDecoratorTests : DecoratorTestBase
	{
		/// <inheritdoc />
		protected override Type DecoratorType => typeof(CachingRepositoryDecorator<,>);

		/// <inheritdoc />
		protected override Type RepositoryType => typeof(NoopTestRepository<Person, Guid>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IRepositoryRegistry, TestRepositoryRegistry>();
			services.AddSingleton<ICachingStrategyFactory, TestCachingStrategyFactory>();
			services.AddSingleton(sp => (TestCachingStrategyFactory)sp.GetRequiredService<ICachingStrategyFactory>());
		}

		private void ShouldHaveUsedStrategy(Func<TestCachingStrategy<Person, Guid>, bool> flagProviderFunc)
		{
			TestCachingStrategyFactory cachingStrategyFactory = this.ServiceProvider.GetRequiredService<TestCachingStrategyFactory>();
			TestCachingStrategy<Person, Guid> cachingStrategy = cachingStrategyFactory.GetStrategy<Person, Guid>();

			bool result = flagProviderFunc.Invoke(cachingStrategy);
			result.Should().BeTrue();
		}

		[Test]
		public async Task Should_AddAsync_Multiple()
		{
			Person[] persons =
			[
				new Person
				{
					Name = "Tester"
				},
				new Person
				{
					Name = "Tester"
				}
			];
			await this.Repository.AddRangeAsync(persons);

			this.ShouldHaveUsedStrategy(x => x.AddMultipleWasCalled);
		}

		[Test]
		public async Task Should_AddAsync_Single()
		{
			await this.Repository.AddAsync(new Person
			{
				Name = "Tester"
			});

			this.ShouldHaveUsedStrategy(x => x.AddSingleWasCalled);
		}

		[Test]
		public async Task Should_CountAsync()
		{
			await this.Repository.CountAsync();

			this.ShouldHaveUsedStrategy(x => x.CountWasCalled);
		}

		[Test]
		public async Task Should_CountAsync_Predicate()
		{
			await this.Repository.CountAsync(x => x.Name == "1");

			this.ShouldHaveUsedStrategy(x => x.CountWithPredicateWasCalled);
		}

		[Test]
		public async Task Should_ExistsAsync_Predicate()
		{
			await this.Repository.ExistsAsync(x => x.Name == "Tester");

			this.ShouldHaveUsedStrategy(x => x.ExistsWithPredicateWasCalled);
		}

		[Test]
		public async Task Should_ExistsAsync_Single()
		{
			await this.Repository.ExistsAsync(Guid.NewGuid());

			this.ShouldHaveUsedStrategy(x => x.ExistsWasCalled);
		}

		[Test]
		public async Task Should_FindManyAsync_Predicate()
		{
			await this.Repository.FindManyAsync(x => x.Name == "1");

			this.ShouldHaveUsedStrategy(x => x.FindManyWithPredicateWasCalled);
		}

		[Test]
		public async Task Should_FindManyAsync_Result()
		{
			await this.Repository.FindManyAsync(x => x.Name == "1", x => x.Name);

			this.ShouldHaveUsedStrategy(x => x.FindManyWithPredicateAndSelectorWasCalled);
		}

		[Test]
		public async Task Should_FindOneAsync_Predicate()
		{
			await this.Repository.FindOneAsync(x => x.Name == "1");

			this.ShouldHaveUsedStrategy(x => x.FindOneWithPredicateWasCalled);
		}

		[Test]
		public async Task Should_FindOneAsync_Result()
		{
			await this.Repository.FindOneAsync(x => x.Name == "1", x => x.Name);

			this.ShouldHaveUsedStrategy(x => x.FindOneWithPredicateAndSelectorWasCalled);
		}

		[Test]
		public async Task Should_GetAsync_Single()
		{
			await this.Repository.GetAsync(Guid.NewGuid());

			this.ShouldHaveUsedStrategy(x => x.GetWasCalled);
		}

		[Test]
		public async Task Should_GetAsync_Single_Result()
		{
			await this.Repository.GetAsync(Guid.NewGuid(), x => x.Name);

			this.ShouldHaveUsedStrategy(x => x.GetWithSelectorWasCalled);
		}

		[Test]
		public async Task Should_RemoveAsync_Multiple_Predicate()
		{
			Person[] persons =
			[
				new Person
				{
					ID = Guid.NewGuid(),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.NewGuid(),
					Name = "Tester"
				}
			];
			await this.Repository.RemoveRangeAsync(persons);

			this.ShouldHaveUsedStrategy(x => x.RemoveMultipleWasCalled);
		}

		[Test]
		public async Task Should_RemoveAsync_Single()
		{
			await this.Repository.RemoveAsync(new Person
			{
				ID = Guid.NewGuid(),
				Name = "Tester"
			});

			this.ShouldHaveUsedStrategy(x => x.RemoveSingleWasCalled);
		}

		[Test]
		public async Task Should_RemoveAsync_Single_Identifier()
		{
			await this.Repository.RemoveAsync(Guid.NewGuid());

			this.ShouldHaveUsedStrategy(x => x.RemoveSingleWasCalled);
		}

		[Test]
		public async Task Should_UpdateAsync_Multiple()
		{
			Person[] persons =
			[
				new Person
				{
					Name = "Tester"
				},
				new Person
				{
					Name = "Tester"
				}
			];
			await this.Repository.UpdateRangeAsync(persons);

			this.ShouldHaveUsedStrategy(x => x.UpdateMultipleWasCalled);
		}

		[Test]
		public async Task Should_UpdateAsync_Single()
		{
			await this.Repository.UpdateAsync(new Person
			{
				ID = Guid.NewGuid(),
				Name = "Tester"
			});

			this.ShouldHaveUsedStrategy(x => x.UpdateSingleWasCalled);
		}
	}
}
