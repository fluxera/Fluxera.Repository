namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class GuardRepositoryDecoratorTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{
			IServiceProvider serviceProvider = BuildServiceProvider(services =>
			{
				services.AddRepository(rb =>
				{
					rb.AddInMemoryRepository("InMemory", rob =>
					{
						rob.UseFor<Person>();

						rob.AddValidation(vob =>
						{
							vob.AddValidatorFactory(vb =>
							{
								vb.AddDataAnnotations(vob.RepositoryName);
							});
						});
					});
				});
			});

			this.repository = serviceProvider.GetRequiredService<IRepository<Person>>();
		}

		private IRepository<Person> repository;

		private void ShouldGuardAgainstDisposed(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				this.repository.Dispose();
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ObjectDisposedException>();
		}

		private void ShouldGuardAgainstNull(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ArgumentNullException>();
		}

		private void ShouldGuardAgainstEmpty(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ArgumentException>();
		}

		private void ShouldGuardAgainstWhitespace(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ArgumentException>();
		}

		private void ShouldGuardAgainstNotTransient(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ArgumentException>();
		}

		private void ShouldGuardAgainstTransient(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				await innerFunc.Invoke();
			};
			func.Should().ThrowAsync<ArgumentException>();
		}

		[Test]
		public void ShouldGuard_AddAsync_Multiple()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.AddAsync(Persons.Null));
			this.ShouldGuardAgainstNotTransient(async () => await this.repository.AddAsync(Persons.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.AddAsync(Persons.Valid));
		}

		[Test]
		public void ShouldGuard_AddAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.AddAsync(Person.Null));
			this.ShouldGuardAgainstNotTransient(async () => await this.repository.AddAsync(Person.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.AddAsync(Person.Valid));
		}

		[Test]
		public void ShouldGuard_CountAsync()
		{
			this.ShouldGuardAgainstDisposed(async () => await this.repository.CountAsync());
		}

		[Test]
		public void ShouldGuard_CountAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.CountAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.CountAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_ExistsAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.ExistsAsync((Expression<Func<Person, bool>>)null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.ExistsAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_ExistsAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.ExistsAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await this.repository.ExistsAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await this.repository.ExistsAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.ExistsAsync("1234"));
		}

		[Test]
		public void ShouldGuard_FindManyAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.FindManyAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.FindManyAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindManyAsync_Result()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.FindManyAsync(null, x => x.Name));
			this.ShouldGuardAgainstNull(async () => await this.repository.FindManyAsync<string>(x => x.Name == "1", null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.FindManyAsync(x => x.Name == "1", x => x.Name));
		}

		[Test]
		public void ShouldGuard_FindOneAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.FindOneAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.FindOneAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindOneAsync_Result()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.FindOneAsync<string>(x => x.Name == "1", null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.FindOneAsync(x => x.Name == "1", x => x.Name));
		}

		[Test]
		public void ShouldGuard_GetAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.GetAsync(null));
			this.ShouldGuardAgainstEmpty(async () => await this.repository.GetAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await this.repository.GetAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.GetAsync("1234"));
		}

		[Test]
		public void ShouldGuard_GetAsync_Single_Result()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.GetAsync(null, x => x.Name));
			this.ShouldGuardAgainstEmpty(async () => await this.repository.GetAsync(string.Empty, x => x.Name));
			this.ShouldGuardAgainstWhitespace(async () => await this.repository.GetAsync("   ", x => x.Name));
			this.ShouldGuardAgainstNull(async () => await this.repository.GetAsync("1234", (Expression<Func<Person, string>>)null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.GetAsync("1234", x => x.Name));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Multiple_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.RemoveAsync((Expression<Func<Person, bool>>)null));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.RemoveAsync(x => x.ID == "1234"));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.RemoveAsync(Person.Null));
			this.ShouldGuardAgainstNotTransient(async () => await this.repository.RemoveAsync(Person.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.RemoveAsync(Person.Valid));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Single_Identifier()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.RemoveAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await this.repository.RemoveAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await this.repository.RemoveAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.RemoveAsync("1234"));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Multiple()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.UpdateAsync(Persons.Null));
			this.ShouldGuardAgainstTransient(async () => await this.repository.UpdateAsync(Persons.Transient));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.UpdateAsync(Persons.Valid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.repository.UpdateAsync(Person.Null));
			this.ShouldGuardAgainstTransient(async () => await this.repository.UpdateAsync(Person.Transient));
			this.ShouldGuardAgainstDisposed(async () => await this.repository.UpdateAsync(Person.Valid));
		}
	}
}
