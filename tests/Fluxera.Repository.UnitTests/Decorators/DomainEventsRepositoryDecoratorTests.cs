namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
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
				builder.AddDomainEventHandler<PersonCommittedDomainEventHandler>();
			});
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
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				},
				new Person
				{
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				}
			};
			await this.Repository.AddRangeAsync(persons);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_AddAsync_Single()
		{
			await this.Repository.AddAsync(new Person
			{
				Name = "Tester",
				DomainEvents =
				{
					new PersonDomainEvent()
				}
			});
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
			await this.Repository.RemoveAsync(new Person
			{
				ID = Guid.NewGuid(),
				Name = "Tester",
				DomainEvents =
				{
					new PersonDomainEvent()
				}
			});
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
					ID = Guid.NewGuid(),
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				},
				new Person
				{
					ID = Guid.NewGuid(),
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				},
				new Person
				{
					ID = Guid.NewGuid(),
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				}
			};
			await this.Repository.UpdateRangeAsync(persons);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_UpdateAsync_Single()
		{
			await this.Repository.UpdateAsync(new Person
			{
				ID = Guid.NewGuid(),
				Name = "Tester",
				DomainEvents =
				{
					new PersonDomainEvent()
				}
			});
			await this.ShouldHaveUsedDispatcher();
		}
	}
}
