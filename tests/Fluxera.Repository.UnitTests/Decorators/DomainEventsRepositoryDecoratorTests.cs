namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class DomainEventsRepositoryDecoratorTests : DecoratorTestBase
	{
		private Mock<ILogger> loggerMock;

		/// <inheritdoc />
		protected override Type DecoratorType => typeof(DomainEventsRepositoryDecorator<>);

		/// <inheritdoc />
		protected override Type RepositoryType => typeof(DomainEventsTestRepository<Person>);

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
				builder.AddDomainEventHandlers<PersonDomainEvent>();
				builder.AddDomainEventHandlers<PersonCommittedDomainEventHandler>();
			});
		}

		private async Task ShouldHaveUsedDispatcher(bool expected = true)
		{
			if(expected)
			{
				// Trace is logged by the decorator.
				this.loggerMock.VerifyTraceWasCalled();

				// Info is logged in the handlers.
				this.loggerMock.VerifyInformationWasCalled();
			}
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
			await this.Repository.AddAsync(persons);
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

			// Trace is logged by the decorator.
			this.loggerMock.VerifyTraceWasCalled();
		}

		[Test]
		public async Task Should_RemoveAsync_Single()
		{
			await this.Repository.RemoveAsync(new Person
			{
				ID = "2",
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
			await this.Repository.RemoveAsync("1");

			// Trace is logged by the decorator.
			this.loggerMock.VerifyTraceWasCalled();
		}

		[Test]
		public async Task Should_UpdateAsync_Multiple()
		{
			Person[] persons =
			{
				new Person
				{
					ID = "1",
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				},
				new Person
				{
					ID = "2",
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				},
				new Person
				{
					ID = "3",
					Name = "Tester",
					DomainEvents =
					{
						new PersonDomainEvent()
					}
				}
			};
			await this.Repository.UpdateAsync(persons);
			await this.ShouldHaveUsedDispatcher();
		}

		[Test]
		public async Task Should_UpdateAsync_Single()
		{
			await this.Repository.UpdateAsync(new Person
			{
				ID = "2",
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
