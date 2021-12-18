namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class AggregateTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldCount()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			long count = await this.Repository.CountAsync();
			count.Should().Be(persons.Length);
		}

		[Test]
		public async Task ShouldCountWithPredicate()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.Repository.AddAsync(persons);
			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			long count = await this.Repository.CountAsync(x => x.Name.EndsWith("2"));
			count.Should().Be(2);
		}
	}
}
