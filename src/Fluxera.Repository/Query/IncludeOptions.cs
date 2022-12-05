namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	internal sealed class IncludeOptions<T> : IIncludeOptions<T> where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;

		private readonly IIncludeApplier includeApplier;

		private readonly IList<Expression<Func<T, object>>> includeExpressions = new List<Expression<Func<T, object>>>();

		public IncludeOptions(QueryOptionsImpl<T> queryOptions, Expression<Func<T, object>> includeExpression, IIncludeApplier includeApplier)
		{
			this.queryOptions = queryOptions;
			this.includeApplier = includeApplier;

			this.includeExpressions.Add(includeExpression);
		}

		/// <inheritdoc />
		public IIncludeOptions<T> Include(Expression<Func<T, object>> includeExpression)
		{
			this.includeExpressions.Add(includeExpression);

			return this;
		}

		/// <inheritdoc />
		public ISortingOptions<T> OrderBy(Expression<Func<T, object>> sortExpression)
		{
			this.queryOptions.SortingOptions ??= new SortingOptions<T>(this.queryOptions, sortExpression, false);
			return this.queryOptions.SortingOptions;
		}

		/// <inheritdoc />
		public ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			this.queryOptions.SortingOptions ??= new SortingOptions<T>(this.queryOptions, sortExpression, true);
			return this.queryOptions.SortingOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Skip(int skip)
		{
			this.queryOptions.SkipTakeOptions ??= new SkipTakeOptions<T>(this.queryOptions, skip);
			return this.queryOptions.SkipTakeOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Take(int take)
		{
			this.queryOptions.SkipTakeOptions ??= new SkipTakeOptions<T>(this.queryOptions, take: take);
			return this.queryOptions.SkipTakeOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> SkipTake(int skip, int take)
		{
			this.queryOptions.SkipTakeOptions ??= new SkipTakeOptions<T>(this.queryOptions, skip, take);
			return this.queryOptions.SkipTakeOptions;
		}

		/// <inheritdoc />
		public IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.queryOptions.PagingOptions ??= new PagingOptions<T>(this.queryOptions, pageNumber, pageSize);
			return this.queryOptions.PagingOptions;
		}

		/// <inheritdoc />
		public IPagingOptions<T> Paging()
		{
			this.queryOptions.PagingOptions ??= new PagingOptions<T>(this.queryOptions);
			return this.queryOptions.PagingOptions;
		}

		/// <inheritdoc />
		public IQueryOptions<T> Build()
		{
			return this.queryOptions;
		}

		/// <inheritdoc />
		IQueryable<T> IIncludeOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			return this.includeApplier?.ApplyTo(queryable, this.includeExpressions) ?? queryable;
		}
	}
}