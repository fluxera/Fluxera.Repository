namespace Fluxera.Repository.UnitTests.Query
{
	using FluentAssertions;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using NUnit.Framework;

	[TestFixture]
	public class QueryOptionsTests
	{
		public class Customer : AggregateRoot<Customer, string>
		{
			public string Name { get; set; }
		}

		[Test]
		public void ShouldBeEmpty()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.Empty();
			queryOptions.IsEmpty().Should().BeTrue();
		}

		[Test]
		public void ShouldBePagingOptions()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.Paging(1, 10);
			queryOptions.Should().BeOfType<PagingOptions<Customer>>();
		}

		[Test]
		public void ShouldBeSkipTakeOptions()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.Skip(5);
			queryOptions.Should().BeOfType<SkipTakeOptions<Customer>>();
		}

		[Test]
		public void ShouldBeSortingOptions()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.OrderBy(x => x.Name);
			queryOptions.Should().BeOfType<SortingOptions<Customer>>();
		}

		[Test]
		public void ShouldHaveSortingAndPagingOptions()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.OrderBy(x => x.Name).Paging(1, 10);
			queryOptions.TryGetSortingOptions(out _).Should().BeTrue();
			queryOptions.TryGetPagingOptions(out IPagingOptions<Customer>? pagingOptions).Should().BeTrue();
			queryOptions.Should().BeSameAs(pagingOptions);
		}

		[Test]
		public void ShouldHaveSortingAndSkipTakeOptions()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.OrderBy(x => x.Name).Take(10);
			queryOptions.TryGetSortingOptions(out _).Should().BeTrue();
			queryOptions.TryGetSkipTakeOptions(out ISkipTakeOptions<Customer>? skipTakeOptions).Should().BeTrue();
			queryOptions.Should().BeSameAs(skipTakeOptions);
		}

		[Test]
		public void ShouldNotBeEmpty()
		{
			IQueryOptions<Customer> queryOptions = QueryOptions<Customer>.OrderBy(x => x.Name);
			queryOptions.IsEmpty().Should().BeFalse();
		}
	}
}
