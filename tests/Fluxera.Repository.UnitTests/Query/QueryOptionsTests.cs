namespace Fluxera.Repository.UnitTests.Query
{
	using System;
	using System.Linq.Expressions;
	using AutoMapper;
	using AutoMapper.Extensions.ExpressionMapping;
	using FluentAssertions;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Query.Impl;
	using NUnit.Framework;

	[TestFixture]
	public class QueryOptionsTests
	{
		private IMapper mapper;
		private MapperConfiguration config;

		[SetUp]
		public void Setup()
		{
			this.config = new MapperConfiguration(cfg => cfg.AddProfile<PersonProfile>());
			this.mapper = this.config.CreateMapper();
		}

		[Test]
		public void ShouldConvertQueryOptions_OnlyPaging()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.Paging(5, 10)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.PagingOptions.Should().NotBeNull();

			PagingOptions<Person> mappedPagingOptions = (PagingOptions<Person>)mappedQueryOptions.PagingOptions;
			mappedPagingOptions.PageNumberAmount.Should().Be(5);
			mappedPagingOptions.PageSizeAmount.Should().Be(10);
		}

		[Test]
		public void ShouldConvertQueryOptions_OnlySkip()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.Skip(10)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.SkipTakeOptions.Should().NotBeNull();

			SkipTakeOptions<Person> mappedSkipTakeOptions = (SkipTakeOptions<Person>)mappedQueryOptions.SkipTakeOptions;
			mappedSkipTakeOptions.SkipAmount.Should().NotBeNull().And.Be(10);
			mappedSkipTakeOptions.TakeAmount.Should().BeNull();
		}

		[Test]
		public void ShouldConvertQueryOptions_OnlySkipTake()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.Skip(50)
				.Take(10)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.SkipTakeOptions.Should().NotBeNull();

			SkipTakeOptions<Person> mappedSkipTakeOptions = (SkipTakeOptions<Person>)mappedQueryOptions.SkipTakeOptions;
			mappedSkipTakeOptions.SkipAmount.Should().NotBeNull().And.Be(50);
			mappedSkipTakeOptions.TakeAmount.Should().NotBeNull().And.Be(10);
		}

		[Test]
		public void ShouldConvertQueryOptions_OnlySorting()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.OrderBy(x => x.Lastname)
				.ThenByDescending(x => x.Age)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.SortingOptions.Should().NotBeNull();

			SortingOptions<Person> mappedSortingOptions = (SortingOptions<Person>)mappedQueryOptions.SortingOptions;
			mappedSortingOptions.PrimaryExpression.Should().NotBeNull();
			mappedSortingOptions.SecondaryExpressions.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldConvertQueryOptions_OnlyTake()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.Take(10)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.SkipTakeOptions.Should().NotBeNull();

			SkipTakeOptions<Person> mappedSkipTakeOptions = (SkipTakeOptions<Person>)mappedQueryOptions.SkipTakeOptions;
			mappedSkipTakeOptions.TakeAmount.Should().NotBeNull().And.Be(10);
			mappedSkipTakeOptions.SkipAmount.Should().BeNull();
		}

		[Test]
		public void ShouldConvertQueryOptions_SortingAndPaging()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.OrderBy(x => x.Lastname)
				.ThenByDescending(x => x.Age)
				.Paging(5, 10)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.SortingOptions.Should().NotBeNull();
			mappedQueryOptions.PagingOptions.Should().NotBeNull();

			SortingOptions<Person> mappedSortingOptions = (SortingOptions<Person>)mappedQueryOptions.SortingOptions;
			mappedSortingOptions.PrimaryExpression.Should().NotBeNull();
			mappedSortingOptions.SecondaryExpressions.Should().NotBeNullOrEmpty();

			PagingOptions<Person> mappedPagingOptions = (PagingOptions<Person>)mappedQueryOptions.PagingOptions;
			mappedPagingOptions.PageNumberAmount.Should().Be(5);
			mappedPagingOptions.PageSizeAmount.Should().Be(10);
		}

		[Test]
		public void ShouldConvertQueryOptions_SortingAndSkipTake()
		{
			IQueryOptions<PersonDto> queryOptions = QueryOptionsBuilder.CreateFor<PersonDto>()
				.OrderBy(x => x.Lastname)
				.ThenByDescending(x => x.Age)
				.Skip(50)
				.Take(10)
				.Build();

			QueryOptionsImpl<Person> mappedQueryOptions =
				(QueryOptionsImpl<Person>)queryOptions.Convert(this.mapper.MapExpression<Expression<Func<Person, object>>>);

			mappedQueryOptions.Should().NotBeNull();
			mappedQueryOptions.SortingOptions.Should().NotBeNull();
			mappedQueryOptions.SkipTakeOptions.Should().NotBeNull();

			SortingOptions<Person> mappedSortingOptions = (SortingOptions<Person>)mappedQueryOptions.SortingOptions;
			mappedSortingOptions.PrimaryExpression.Should().NotBeNull();
			mappedSortingOptions.SecondaryExpressions.Should().NotBeNullOrEmpty();

			SkipTakeOptions<Person> mappedSkipTakeOptions = (SkipTakeOptions<Person>)mappedQueryOptions.SkipTakeOptions;
			mappedSkipTakeOptions.SkipAmount.Should().NotBeNull().And.Be(50);
			mappedSkipTakeOptions.TakeAmount.Should().NotBeNull().And.Be(10);
		}
	}

	public class PersonProfile : Profile
	{
		public PersonProfile()
		{
			this.CreateMap<PersonDto, Person>().ReverseMap();
		}
	}

	public class Person : Entity<Person, string>
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public int Age { get; set; }
	}

	public class PersonDto
	{
		public string ID { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public int Age { get; set; }
	}
}
