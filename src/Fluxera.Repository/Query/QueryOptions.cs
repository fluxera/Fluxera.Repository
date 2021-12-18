namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class QueryOptions<T> where T : class
	{
		public static IQueryOptions<T> Empty()
		{
			return new EmptyQueryOptions<T>();
		}

		public static ISortingOptions<T> OrderBy(Expression<Func<T, object>> sortExpression)
		{
			return new SortingOptions<T>(sortExpression);
		}

		public static ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			return new SortingOptions<T>(sortExpression, true);
		}

		public static ISkipTakeOptions<T> Skip(int skipAmount)
		{
			return new SkipTakeOptions<T>(skipAmount);
		}

		public static ISkipTakeOptions<T> Take(int takeAmount)
		{
			return new SkipTakeOptions<T>(take: takeAmount);
		}

		public static ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount)
		{
			return new SkipTakeOptions<T>(skipAmount, takeAmount);
		}

		public static IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			return new PagingOptions<T>(pageNumber, pageSize);
		}

		public static IPagingOptions<T> Paging()
		{
			return new PagingOptions<T>();
		}
	}
}
