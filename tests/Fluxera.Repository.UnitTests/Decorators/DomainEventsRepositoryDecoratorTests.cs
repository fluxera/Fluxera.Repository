namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.DomainEvents;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Logging.Mock;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class DomainEventsRepositoryDecoratorTests : DecoratorTestBase
	{
		private Mock<ILogger> loggerMock;

		/// <inheritdoc />
		protected override Type DecoratorType => typeof(DomainEventsRepositoryDecorator<,>);

		/// <inheritdoc />
		protected override Type RepositoryType => typeof(DomainEventsTestRepository<Person, Guid>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			this.loggerMock = new Mock<ILogger>();

			services.AddLogging(builder =>
			{
				builder.AddMock(this.loggerMock);
			});

			services.AddDomainEvents(builder =>
			{
				builder.AddDomainEventHandler<PersonDomainEventHandler>();
			});

			services.AddSingleton<IRepositoryRegistry, TestRepositoryRegistry>();
			services.AddSingleton<ICrudDomainEventsFactory, TestCrudDomainEventsFactory>();
		}

		private Task ShouldHaveUsedDispatcher(bool expected = true)
		{
			if(expected)
			{
				// Debug is logged by the decorator.
				this.loggerMock.VerifyLog().DebugWasCalled();

				// Info is logged in the handlers.
				this.loggerMock.VerifyLog().InformationWasCalled();
			}

			return Task.CompletedTask;
		}

		[Test]
		public async Task Should_AddAsync_Multiple()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester"
				},
				new Person
				{
					Name = "Tester"
				}
			};
			persons.ForEach(x => x.RaiseDomainEvent(new PersonDomainEvent()));

			await this.Repository.AddRangeAsync(persons);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_AddAsync_Single()
		{
			Person person = new Person
			{
				Name = "Tester"
			};
			person.RaiseDomainEvent(new PersonDomainEvent());

			await this.Repository.AddAsync(person);
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
			await this.Repository.ExistsAsync(Guid.NewGuid());
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
			await this.Repository.GetAsync(Guid.NewGuid());
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_GetAsync_Single_Result()
		{
			await this.Repository.GetAsync(Guid.NewGuid(), x => x.Name);
			await this.ShouldHaveUsedDispatcher(false);
		}

		[Test]
		public async Task Should_RemoveAsync_Multiple_Predicate()
		{
			await this.Repository.RemoveRangeAsync(x => x.Name == "1");

			// Debug is logged by the decorator.
			this.loggerMock.VerifyLog().DebugWasCalled();
		}

		[Test]
		public async Task Should_RemoveAsync_Single()
		{
			Person person = new Person
			{
				ID = Guid.Parse("d37eaa47-7cb0-4368-af1a-8f1c94be9782"),
				Name = "Tester"
			};
			person.RaiseDomainEvent(new PersonDomainEvent());

			await this.Repository.RemoveAsync(person);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_RemoveAsync_Single_Identifier()
		{
			await this.Repository.RemoveAsync(Guid.NewGuid());

			// Debug is logged by the decorator.
			this.loggerMock.VerifyLog().DebugWasCalled();
		}

		[Test]
		public async Task Should_UpdateAsync_Multiple()
		{
			Person[] persons =
			{
				new Person
				{
					ID = Guid.Parse("8693cbd0-a564-47cf-9fe3-b1444392957d"),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.Parse("c8fbfccd-a14c-41ba-8e2f-d32b286b6804"),
					Name = "Tester"
				},
				new Person
				{
					ID = Guid.Parse("fabb0b65-45c5-4aff-87b0-45b766074588"),
					Name = "Tester"
				}
			};
			persons.ForEach(x => x.RaiseDomainEvent(new PersonDomainEvent()));

			await this.Repository.UpdateRangeAsync(persons);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_UpdateAsync_Single()
		{
			Person person = new Person
			{
				ID = Guid.Parse("d37eaa47-7cb0-4368-af1a-8f1c94be9782"),
				Name = "Tester"
			};
			person.RaiseDomainEvent(new PersonDomainEvent());

			await this.Repository.UpdateAsync(person);
			await this.ShouldHaveUsedDispatcher();
		}
	}
}
