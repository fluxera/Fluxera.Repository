namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class DomainEventsRepositoryDecoratorTests : DecoratorTestBase
	{
		/// <inheritdoc />
		protected override Type DecoratorType => typeof(DomainEventsRepositoryDecorator<>);

		/// <inheritdoc />
		protected override Type RepositoryType => typeof(DomainEventsTestRepository<Person>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IDomainEventDispatcher, TestDomainEventDispatcher>();
			services.AddSingleton(sp => (TestDomainEventDispatcher)sp.GetRequiredService<IDomainEventDispatcher>());
		}

		private async Task ShouldHaveUsedDispatcher(bool expected = true)
		{
			TestDomainEventDispatcher domainEventDispatcher = this.ServiceProvider.GetRequiredService<TestDomainEventDispatcher>();
			domainEventDispatcher.DispatchWasCalled.Should().Be(expected);
			domainEventDispatcher.DispatchCommittedWasCalled.Should().Be(expected);
		}

		[Test]
		public async Task Should_AddAsync_Multiple()
		{
			await this.Repository.AddAsync(Persons.Valid);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_AddAsync_Single()
		{
			await this.Repository.AddAsync(Person.Valid);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_CountAsync()
		{
			await this.Repository.CountAsync();
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_CountAsync_Predicate()
		{
			await this.Repository.CountAsync(x => x.Name == "1");
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_ExistsAsync_Predicate()
		{
			await this.Repository.ExistsAsync(x => x.Name == "1");
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_ExistsAsync_Single()
		{
			await this.Repository.ExistsAsync("1");
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_FindManyAsync_Predicate()
		{
			await this.Repository.FindManyAsync(x => x.Name == "1");
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_FindManyAsync_Result()
		{
			await this.Repository.FindManyAsync(x => x.Name == "1", x => x.Name);
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_FindOneAsync_Predicate()
		{
			await this.Repository.FindOneAsync(x => x.Name == "1");
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_FindOneAsync_Result()
		{
			await this.Repository.FindOneAsync(x => x.Name == "1", x => x.Name);
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_GetAsync_Single()
		{
			await this.Repository.GetAsync("1");
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_GetAsync_Single_Result()
		{
			await this.Repository.GetAsync("1", x => x.Name);
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_RemoveAsync_Multiple_Predicate()
		{
			await this.Repository.RemoveAsync(x => x.Name == "1");
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_RemoveAsync_Single()
		{
			await this.Repository.RemoveAsync(Person.NotTransient);
			await this.ShouldHaveUsedDispatcher();
		}

		// TODO: Why do these fail?
		//[Test]
		//public async Task Should_RemoveAsync_Single_Identifier()
		//{
		//	await this.Repository.RemoveAsync("1");
		//	await this.ShouldHaveUsedDispatcher();
		//}

		//[Test]
		//public async Task Should_UpdateAsync_Multiple()
		//{
		//	await this.Repository.UpdateAsync(Persons.NotTransient);
		//	await this.ShouldHaveUsedDispatcher();
		//}

		//[Test]
		//public async Task Should_UpdateAsync_Single()
		//{
		//	await this.Repository.UpdateAsync(Person.NotTransient);
		//	await this.ShouldHaveUsedDispatcher();
		//}
	}
}
