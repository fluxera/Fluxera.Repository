namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A helper service to create entry points for building <see cref="IQueryOptions{T}" />.
	/// </summary>
	[PublicAPI]
	public class QueryOptionsBuilder<T> where T : class
	{
		private QueryOptionsImpl<T> queryOptions;

		private readonly IIncludeApplier includeApplier;

		/// <summary>
		///     Initializes a new instance of the <see cref="QueryOptionsBuilder{T}" /> type.
		/// </summary>
		/// <param name="queryApplierFactory"></param>
		/// <param name="repositoryRegistry"></param>
		public QueryOptionsBuilder(IQueryApplierFactory queryApplierFactory, IRepositoryRegistry repositoryRegistry)
		{
			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<T>();
			this.includeApplier = queryApplierFactory.CreateIncludeApplier(repositoryName);
		}

		/// <summary>
		///     Creates an empty options instance.
		/// </summary>
		/// <returns></returns>
		public static IQueryOptions<T> Empty()
		{
			return QueryOptions<T>.Empty();
		}

		/// <summary>
		///     Creates an entry point for configuring the include options.
		/// </summary>
		/// <param name="includeExpression"></param>
		/// <returns></returns>
		public IIncludeOptions<T> Include(Expression<Func<T, object>> includeExpression)
		{
			this.queryOptions = new QueryOptionsImpl<T>(this.includeApplier);
			return queryOptions.Include(includeExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public ISortingOptions<T> OrderBy<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.OrderBy(sortExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the sorting options.
		/// </summary>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.OrderByDescending(sortExpression);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <returns></returns>
		public ISkipTakeOptions<T> Skip(int skipAmount)
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Skip(skipAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		public ISkipTakeOptions<T> Take(int takeAmount)
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Take(takeAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the skip/take options.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		public ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount)
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.SkipTake(skipAmount, takeAmount);
		}

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Paging(pageNumber, pageSize);
		}

		/// <summary>
		///     Creates an entry point for configuring the paging options.
		/// </summary>
		/// <returns></returns>
		public IPagingOptions<T> Paging()
		{
			this.queryOptions = new QueryOptionsImpl<T>();
			return queryOptions.Paging();
		}

		/// <summary>
		///     Builds the query options instance.
		/// </summary>
		/// <returns></returns>
		public IQueryOptions<T> Build()
		{
			return this.queryOptions;
		}
	}
}
