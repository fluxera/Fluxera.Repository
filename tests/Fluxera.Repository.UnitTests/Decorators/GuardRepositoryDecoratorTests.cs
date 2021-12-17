namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using NUnit.Framework;

	[TestFixture]
	public class GuardRepositoryDecoratorTests : DecoratorTestBase
	{
		private void ShouldGuardAgainstDisposed(Func<Task> innerFunc)
		{
			Func<Task> func = async () =>
			{
				this.Repository.Dispose();
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

		/// <inheritdoc />
		protected override Type DecoratorType => typeof(GuardRepositoryDecorator<,>);

		[Test]
		public void ShouldGuard_AddAsync_Multiple()
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
			this.ShouldGuardAgainstNull(async () => await this.Repository.AddAsync((Person[])null));
			this.ShouldGuardAgainstNotTransient(async () => await this.Repository.AddAsync(persons));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.AddAsync(persons));
		}

		[Test]
		public void ShouldGuard_AddAsync_Single()
		{
			Person person = new Person
			{
				ID = "1",
				Name = "Tester"
			};
			this.ShouldGuardAgainstNull(async () => await this.Repository.AddAsync((Person)null));
			this.ShouldGuardAgainstNotTransient(async () => await this.Repository.AddAsync(person));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.AddAsync(person));
		}

		[Test]
		public void ShouldGuard_CountAsync()
		{
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.CountAsync());
		}

		[Test]
		public void ShouldGuard_CountAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.CountAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.CountAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_ExistsAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.ExistsAsync((Expression<Func<Person, bool>>)null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.ExistsAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_ExistsAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.ExistsAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await this.Repository.ExistsAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await this.Repository.ExistsAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.ExistsAsync("1234"));
		}

		[Test]
		public void ShouldGuard_FindManyAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.FindManyAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.FindManyAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindManyAsync_Result()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.FindManyAsync(null, x => x.Name));
			this.ShouldGuardAgainstNull(async () => await this.Repository.FindManyAsync<string>(x => x.Name == "1", null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.FindManyAsync(x => x.Name == "1", x => x.Name));
		}

		[Test]
		public void ShouldGuard_FindOneAsync_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.FindOneAsync(null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.FindOneAsync(x => x.Name == "1"));
		}

		[Test]
		public void ShouldGuard_FindOneAsync_Result()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.FindOneAsync<string>(x => x.Name == "1", null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.FindOneAsync(x => x.Name == "1", x => x.Name));
		}

		[Test]
		public void ShouldGuard_GetAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.GetAsync(null));
			this.ShouldGuardAgainstEmpty(async () => await this.Repository.GetAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await this.Repository.GetAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.GetAsync("1234"));
		}

		[Test]
		public void ShouldGuard_GetAsync_Single_Result()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.GetAsync(null, x => x.Name));
			this.ShouldGuardAgainstEmpty(async () => await this.Repository.GetAsync(string.Empty, x => x.Name));
			this.ShouldGuardAgainstWhitespace(async () => await this.Repository.GetAsync("   ", x => x.Name));
			this.ShouldGuardAgainstNull(async () => await this.Repository.GetAsync("1234", (Expression<Func<Person, string>>)null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.GetAsync("1234", x => x.Name));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Multiple_Predicate()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.RemoveAsync((Expression<Func<Person, bool>>)null));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.RemoveAsync(x => x.ID == "1234"));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Single()
		{
			Person person = new Person
			{
				ID = "1",
				Name = "Tester"
			};
			this.ShouldGuardAgainstNull(async () => await this.Repository.RemoveAsync((Person)null));
			this.ShouldGuardAgainstNotTransient(async () => await this.Repository.RemoveAsync(person));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.RemoveAsync(person));
		}

		[Test]
		public void ShouldGuard_RemoveAsync_Single_Identifier()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.RemoveAsync((string)null));
			this.ShouldGuardAgainstEmpty(async () => await this.Repository.RemoveAsync(string.Empty));
			this.ShouldGuardAgainstWhitespace(async () => await this.Repository.RemoveAsync("   "));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.RemoveAsync("1234"));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Multiple()
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
			Person[] validPersons =
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
			this.ShouldGuardAgainstNull(async () => await this.Repository.UpdateAsync((Person[])null));
			this.ShouldGuardAgainstTransient(async () => await this.Repository.UpdateAsync(persons));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.UpdateAsync(validPersons));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.UpdateAsync((Person)null));
			this.ShouldGuardAgainstTransient(async () => await this.Repository.UpdateAsync(new Person
			{
				Name = "Tester"
			}));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.UpdateAsync(new Person
			{
				ID = "1",
				Name = "Tester"
			}));
		}
	}
}
