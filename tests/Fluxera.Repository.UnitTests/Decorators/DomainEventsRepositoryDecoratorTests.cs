namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.Decorators;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class DomainEventsRepositoryDecoratorTests : DecoratorTestBase
	{
		/// <inheritdoc />
		protected override Type DecoratorType => typeof(DomainEventsRepositoryDecorator<>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<IDomainEventDispatcher, TestDomainEventDispatcher>();
		}

		[Test]
		public async Task Should_AddAsync_Multiple()
		{
		}

		[Test]
		public async Task Should_AddAsync_Single()
		{
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
		}

		[Test]
		public async Task Should_UpdateAsync_Single()
		{
		}
	}
}
