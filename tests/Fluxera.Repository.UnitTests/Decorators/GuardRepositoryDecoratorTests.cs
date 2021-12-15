namespace Fluxera.Repository.UnitTests.Decorators
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Decorators;
	using Fluxera.Repository.UnitTests.PersonAggregate;
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
		protected override Type DecoratorType => typeof(GuardRepositoryDecorator<>);

		[Test]
		public void ShouldGuard_AddAsync_Multiple()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.AddAsync(Persons.Null));
			this.ShouldGuardAgainstNotTransient(async () => await this.Repository.AddAsync(Persons.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.AddAsync(Persons.Valid));
		}

		[Test]
		public void ShouldGuard_AddAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.AddAsync(Person.Null));
			this.ShouldGuardAgainstNotTransient(async () => await this.Repository.AddAsync(Person.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.AddAsync(Person.Valid));
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
			this.ShouldGuardAgainstNull(async () => await this.Repository.RemoveAsync(Person.Null));
			this.ShouldGuardAgainstNotTransient(async () => await this.Repository.RemoveAsync(Person.NotTransient));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.RemoveAsync(Person.Valid));
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
			this.ShouldGuardAgainstNull(async () => await this.Repository.UpdateAsync(Persons.Null));
			this.ShouldGuardAgainstTransient(async () => await this.Repository.UpdateAsync(Persons.Transient));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.UpdateAsync(Persons.Valid));
		}

		[Test]
		public void ShouldGuard_UpdateAsync_Single()
		{
			this.ShouldGuardAgainstNull(async () => await this.Repository.UpdateAsync(Person.Null));
			this.ShouldGuardAgainstTransient(async () => await this.Repository.UpdateAsync(Person.Transient));
			this.ShouldGuardAgainstDisposed(async () => await this.Repository.UpdateAsync(Person.Valid));
		}
	}
}
