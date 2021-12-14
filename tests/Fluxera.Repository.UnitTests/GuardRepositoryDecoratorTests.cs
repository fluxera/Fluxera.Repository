namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Extensions.Validation.DataAnnotations;
	using Fluxera.Repository.Storage.InMemory;
	using Fluxera.Repository.UnitTests.PersonAggregate;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class GuardRepositoryDecoratorTests : TestBase
	{
		private IRepository<Person> repository;

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

		[Test]
		public void ShouldGuard_AddAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await repository.AddAsync(Person.Null));
			this.ShouldGuardAgainstNotTransient(async () => await repository.AddAsync(Person.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await repository.AddAsync(Person.Valid));
		}

		[Test]
		public void ShouldGuard_AddAsync_Multiple()
		{
			this.ShouldGuardAgainstNull(async () => await repository.AddAsync(Persons.Null));
			this.ShouldGuardAgainstNotTransient(async () => await repository.AddAsync(Persons.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await repository.AddAsync(Persons.Valid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await repository.UpdateAsync(Person.Null));
			this.ShouldGuardAgainstTransient(async () => await repository.UpdateAsync(Person.Transient));
			this.ShouldGuardAgainstDisposed(async () => await repository.UpdateAsync(Person.Valid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Multiple()
		{
			this.ShouldGuardAgainstNull(async () => await repository.UpdateAsync(Persons.Null));
			this.ShouldGuardAgainstTransient(async () => await repository.UpdateAsync(Persons.Transient));
			this.ShouldGuardAgainstDisposed(async () => await repository.UpdateAsync(Persons.Valid));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await repository.RemoveAsync(Person.Null));
			this.ShouldGuardAgainstNotTransient(async () => await repository.RemoveAsync(Person.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await repository.RemoveAsync(Person.Valid));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Single_Identifier()
		{
			this.ShouldGuardAgainstNull(async () => await repository.RemoveAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await repository.RemoveAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await repository.RemoveAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await repository.RemoveAsync("1234"));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Multiple_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await repository.RemoveAsync((Expression<Func<Person, bool>>)null));
			this.ShouldGuardAgainstDisposed(async () => await repository.RemoveAsync(x => x.ID == "1234"));
		}

		[Test]
		public void ShouldGuard_GetAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await repository.GetAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await repository.GetAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await repository.GetAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await repository.GetAsync("1234"));
		}

		[Test]
		public void ShouldGuard_GetAsync_Single_Result()
		{
			this.ShouldGuardAgainstNull(async () => await repository.GetAsync((string)null, x => x.Name));
			this.ShouldGuardAgainstEmpty(async () => await repository.GetAsync(string.Empty, x => x.Name));
			this.ShouldGuardAgainstWhitespace(async () => await repository.GetAsync("   ", x => x.Name));
			this.ShouldGuardAgainstNull(async () => await repository.GetAsync("1234", (Expression<Func<Person, string>>)null));
			this.ShouldGuardAgainstDisposed(async () => await repository.GetAsync("1234", x => x.Name));
		}

		[Test]
		public void ShouldGuard_ExistsAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await repository.ExistsAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await repository.ExistsAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await repository.ExistsAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await repository.ExistsAsync("1234"));
		}

		[Test]
		public void ShouldGuard_ExistsAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await repository.ExistsAsync((Expression<Func<Person, bool>>)null));
			this.ShouldGuardAgainstDisposed(async () => await repository.ExistsAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_CountAsync()
		{
			this.ShouldGuardAgainstDisposed(async () => await repository.CountAsync());
		}
	
		[Test]
		public void ShouldGuard_CountAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await repository.CountAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await repository.CountAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindOneAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await repository.FindOneAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await repository.FindOneAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindOneAsync_Result()
		{
			this.ShouldGuardAgainstNull(async () => await repository.FindOneAsync<string>(x => x.Name == "1", selector:null));
			this.ShouldGuardAgainstDisposed(async () => await repository.FindOneAsync(x => x.Name == "1", x => x.Name));
		}

		[Test]
		public void ShouldGuard_FindManyAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await repository.FindManyAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await repository.FindManyAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindManyAsync_Result()
		{
			this.ShouldGuardAgainstNull(async () => await repository.FindManyAsync(null, x => x.Name));
			this.ShouldGuardAgainstNull(async () => await repository.FindManyAsync<string>(x => x.Name == "1", selector:null));
			this.ShouldGuardAgainstDisposed(async () => await repository.FindManyAsync(x => x.Name == "1", x => x.Name));
		}

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
	}
}
