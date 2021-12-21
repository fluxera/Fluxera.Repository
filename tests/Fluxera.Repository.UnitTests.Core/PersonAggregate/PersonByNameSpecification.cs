namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Repository.Specifications;

	public sealed class PersonByNameSpecification : Specification<Person>
	{
		private readonly string name;

		public PersonByNameSpecification(string name)
		{
			this.name = name;
		}

		/// <inheritdoc />
		protected override Expression<Func<Person, bool>> BuildQuery()
		{
			return x => x.Name == this.name;
		}
	}

	public sealed class PersonByAgeSpecification : Specification<Person>
	{
		private readonly int age;

		public PersonByAgeSpecification(int age)
		{
			this.age = age;
		}

		/// <inheritdoc />
		protected override Expression<Func<Person, bool>> BuildQuery()
		{
			return x => x.Age == this.age;
		}
	}
}
