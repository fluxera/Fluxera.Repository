namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Logging.Mock;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class ExceptionLoggingRepositoryDecoratorTests : DecoratorTestBase
	{
		private Mock<ILogger> loggerMock;

		/// <inheritdoc />
		protected override Type DecoratorType => typeof(ExceptionLoggingRepositoryDecorator<>);

		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection services)
		{
			this.loggerMock = new Mock<ILogger>();

			services.AddLogging(builder =>
			{
				builder.AddMock(this.loggerMock);
			});
		}

		private async Task ShouldLogException(Func<Task> func)
		{
			this.loggerMock.Reset();

			await func.Should().ThrowAsync<NotImplementedException>();

			this.loggerMock.VerifyLog().CriticalWasCalled();
		}

		[Test]
		public async Task ShouldLogException_AddAsync_Multiple()
		{
			await this.ShouldLogException(async () =>
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
				await this.Repository.AddAsync(persons);
			});
		}

		[Test]
		public async Task ShouldLogException_AddAsync_Single()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.AddAsync(new Person
				{
					Name = "Tester"
				});
			});
		}

		[Test]
		public async Task ShouldLogException_CountAsync()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.CountAsync();
			});
		}

		[Test]
		public async Task ShouldLogException_CountAsync_Predicate()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.CountAsync(x => x.Name == "Tester");
			});
		}

		[Test]
		public async Task ShouldLogException_ExistsAsync_Predicate()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.ExistsAsync(x => x.Name == "Tester");
			});
		}

		[Test]
		public async Task ShouldLogException_ExistsAsync_Single()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.ExistsAsync("1");
			});
		}

		[Test]
		public async Task ShouldLogException_FindManyAsync_Predicate()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.FindManyAsync(x => x.Name == "Tester");
			});
		}

		[Test]
		public async Task ShouldLogException_FindManyAsync_Result()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.FindManyAsync(x => x.Name == "Tester", x => x.Name);
			});
		}

		[Test]
		public async Task ShouldLogException_FindOneAsync_Predicate()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.FindOneAsync(x => x.Name == "Tester");
			});
		}

		[Test]
		public async Task ShouldLogException_FindOneAsync_Result()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.FindOneAsync(x => x.Name == "Tester", x => x.Name);
			});
		}

		[Test]
		public async Task ShouldLogException_GetAsync_Single()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.GetAsync("1");
			});
		}

		[Test]
		public async Task ShouldLogException_GetAsync_Single_Result()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.GetAsync("1", x => x.Name);
			});
		}

		[Test]
		public async Task ShouldLogException_RemoveAsync_Multiple_Predicate()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.RemoveAsync(x => x.Name == "Tester");
			});
		}

		[Test]
		public async Task ShouldLogException_RemoveAsync_Single()
		{
			await this.ShouldLogException(async () =>
			{
				Person[] persons =
				{
					new Person
					{
						ID = "1",
						Name = "Tester"
					},
					new Person
					{
						ID = "2",
						Name = "Tester"
					}
				};
				await this.Repository.RemoveAsync(persons);
			});
		}

		[Test]
		public async Task ShouldLogException_RemoveAsync_Single_Identifier()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.RemoveAsync("1");
			});
		}

		[Test]
		public async Task ShouldLogException_UpdateAsync_Multiple()
		{
			await this.ShouldLogException(async () =>
			{
				Person[] persons =
				{
					new Person
					{
						ID = "1",
						Name = "Tester"
					},
					new Person
					{
						ID = "2",
						Name = "Tester"
					}
				};
				await this.Repository.UpdateAsync(persons);
			});
		}

		[Test]
		public async Task ShouldLogException_UpdateAsync_Single()
		{
			await this.ShouldLogException(async () =>
			{
				await this.Repository.UpdateAsync(new Person
				{
					ID = "1",
					Name = "Tester"
				});
			});
		}
	}
}
