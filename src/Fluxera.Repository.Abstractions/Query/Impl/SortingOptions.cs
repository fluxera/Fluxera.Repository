namespace Fluxera.Repository.Query.Impl
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;

	internal sealed class SortingOptions<T> : ISortingOptions<T> where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;
		private Func<IQueryable<T>, IQueryable<T>> applyAdditionalQueryable;

		public SortingOptions(QueryOptionsImpl<T> queryOptions, ISortExpression<T> primaryExpression)
		{
			this.queryOptions = queryOptions;

			this.PrimaryExpression = primaryExpression;
		}

		/// <inheritdoc />
		public ISortExpression<T> PrimaryExpression { get; }

		/// <inheritdoc />
		public IList<ISortExpression<T>> SecondaryExpressions { get; } = new List<ISortExpression<T>>();

		/// <inheritdoc />
		public ISortingOptions<T> ThenBy<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.SecondaryExpressions.Add(new SortExpression<T, TValue>(sortExpression, false));

			return this;
		}

		/// <inheritdoc />
		public ISortingOptions<T> ThenByDescending<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.SecondaryExpressions.Add(new SortExpression<T, TValue>(sortExpression, true));

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
		public IQueryOptions<T> Build(Func<IQueryable<T>, IQueryable<T>> applyFunc = null)
		{
			this.applyAdditionalQueryable = applyFunc;

			return this.queryOptions;
		}

		/// <inheritdoc />
		IQueryable<T> ISortingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			queryable = this.PrimaryExpression.ApplyTo(queryable);

			foreach(ISortExpression<T> secondaryExpression in this.SecondaryExpressions)
			{
				IOrderedQueryable<T> orderedQueryable = (IOrderedQueryable<T>)queryable;
				queryable = secondaryExpression.ApplyTo(orderedQueryable);
			}

			queryable = this.applyAdditionalQueryable?.Invoke(queryable) ?? queryable;

			return queryable;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			string orderByString = this.PrimaryExpression.ToString();
			string thenByString = this.SecondaryExpressions.Select(x => x.ToString()).Aggregate((s1, s2) => string.Concat(s1, ", ", s2));
			string sortingOptionsString = "(OrderBy: {0}, ThenBy: {1})".FormatInvariantWith(orderByString, thenByString);

			string pagingOptionsString = this.queryOptions.PagingOptions != null ? this.queryOptions.PagingOptions.ToString() : "none";
			string skipTakeOptionsString = this.queryOptions.SkipTakeOptions != null ? this.queryOptions.SkipTakeOptions.ToString() : "none";

			return "QueryOptions<{0}>(Sorting: {1}, Paging: {2}, SkipTake: {3})"
				.FormatInvariantWith(typeof(T).Name, sortingOptionsString, pagingOptionsString, skipTakeOptionsString);
		}
	}
}
