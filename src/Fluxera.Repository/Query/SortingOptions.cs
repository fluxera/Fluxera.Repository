namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;

	internal sealed class SortingOptions<T> : ISortingOptions<T> where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;

		private readonly ISortExpression<T> primaryExpression;

		private readonly IList<ISortExpression<T>> secondaryExpressions = new List<ISortExpression<T>>();

		public SortingOptions(QueryOptionsImpl<T> queryOptions, Expression<Func<T, object>> sortExpression, bool isDescending)
		{
			this.queryOptions = queryOptions;

			this.primaryExpression = new SortExpression<T>(sortExpression, isDescending);
		}

		/// <inheritdoc />
		public ISortingOptions<T> ThenBy(Expression<Func<T, object>> sortExpression)
		{
			this.secondaryExpressions.Add(new SortExpression<T>(sortExpression));

			return this;
		}

		/// <inheritdoc />
		public ISortingOptions<T> ThenByDescending(Expression<Func<T, object>> sortExpression)
		{
			this.secondaryExpressions.Add(new SortExpression<T>(sortExpression, true));

			return this;
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
		IQueryable<T> ISortingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			queryable = this.primaryExpression.ApplyTo(queryable);
			foreach(ISortExpression<T> secondaryExpression in this.secondaryExpressions)
			{
				IOrderedQueryable<T> orderedQueryable = (IOrderedQueryable<T>)queryable;
				queryable = secondaryExpression.ApplyTo(orderedQueryable);
			}

			return queryable;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			string orderByString = this.primaryExpression.ToString();
			string thenByString = this.secondaryExpressions.Select(x => x.ToString()).Aggregate((s1, s2) => string.Concat(s1, ", ", s2));
			string sortingOptionsString = "(OrderBy: {0}, ThenBy: {1})".FormatInvariantWith(orderByString, thenByString);

			string pagingOptionsString = this.queryOptions.PagingOptions != null ? this.queryOptions.PagingOptions.ToString() : "none";
			string skipTakeOptionsString = this.queryOptions.SkipTakeOptions != null ? this.queryOptions.SkipTakeOptions.ToString() : "none";

			return "QueryOptions<{0}>(Sorting: {1}, Paging: {2}, SkipTake: {3})"
				.FormatInvariantWith(typeof(T).Name, sortingOptionsString, pagingOptionsString, skipTakeOptionsString);
		}
	}
}
