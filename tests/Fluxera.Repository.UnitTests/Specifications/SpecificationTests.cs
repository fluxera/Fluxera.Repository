﻿namespace Fluxera.Repository.UnitTests.Specifications
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using NUnit.Framework;

	[TestFixture]
	public class SpecificationTests
	{
		private IQueryable<Person> People
		{
			get
			{
				IList<Person> list = new List<Person>();

				for(int i = 0; i < 25; i++)
				{
					list.Add(new Person
					{
						ID = Guid.NewGuid(),
						Name = $"Tester-{i}",
					});
				}

				return list.AsQueryable();
			}
		}

		[Test]
		public void ShouldSatisfyAll_Default()
		{
			ISpecification<Person> spec = new Specification<Person>();
			IQueryable<Person> queryable = spec.ApplyTo(this.People);

			queryable.Count().Should().Be(25);
		}

		[Test]
		public void ShouldSatisfyAll_Explicit()
		{
			ISpecification<Person> spec = Specification<Person>.All;
			IQueryable<Person> queryable = spec.ApplyTo(this.People);

			queryable.Count().Should().Be(25);
		}

		[Test]
		public void ShouldSatisfyNone()
		{
			ISpecification<Person> spec = Specification<Person>.None;
			IQueryable<Person> queryable = spec.ApplyTo(this.People);

			queryable.Count().Should().Be(0);
		}

		[Test]
		public void ShouldCombineUsingAndOperator()
		{
			PersonByAgeSpecification byAge = new PersonByAgeSpecification(32);
			PersonByNameSpecification byName = new PersonByNameSpecification("Anakin");

			Specification<Person> specification = byAge & byName;

			specification.Should().NotBeNull();
			specification.Should().BeOfType<AndAlsoSpecification<Person>>();
		}

		[Test]
		public void ShouldCombineUsingOrOperator()
		{
			PersonByAgeSpecification byAge = new PersonByAgeSpecification(32);
			PersonByNameSpecification byName = new PersonByNameSpecification("Anakin");

			Specification<Person> specification = byAge | byName;

			specification.Should().NotBeNull();
			specification.Should().BeOfType<OrElseSpecification<Person>>();
		}

		[Test]
		public void ShouldNegateUsingOrOperator()
		{
			PersonByAgeSpecification byAge = new PersonByAgeSpecification(32);

			Specification<Person> specification = !byAge;

			specification.Should().NotBeNull();
			specification.Should().BeOfType<NotSpecification<Person>>();
		}
	}
}
