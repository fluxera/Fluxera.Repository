namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;

	internal sealed class SortingOptions<T> : ISortingOptions<T> where T : class
	{
		private readonly IList<ISortExpression<T>> secondaryExpressions = new List<ISortExpression<T>>();

		private IPagingOptions<T>? pagingOptions;
		private ISkipTakeOptions<T>? skipTakeOptions;

		public SortingOptions(Expression<Func<T, object>> sortExpression, bool isDescending = false)
		{
			this.PrimaryExpression = new SortExpression<T>(sortExpression, isDescending);
		}

		/// <inheritdoc />
		public IEnumerable<ISortExpression<T>> SecondaryExpressions => this.secondaryExpressions;

		/// <inheritdoc />
		public ISortExpression<T> PrimaryExpression { get; }

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
		public IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.pagingOptions = new PagingOptions<T>(pageNumber, pageSize);
			return this.pagingOptions;
		}

		/// <inheritdoc />
		public IPagingOptions<T> Paging()
		{
			this.pagingOptions = new PagingOptions<T>();
			return this.pagingOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Skip(int skip)
		{
			this.skipTakeOptions = new SkipTakeOptions<T>(skip);
			return this.skipTakeOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Take(int take)
		{
			this.skipTakeOptions = new SkipTakeOptions<T>(take: take);
			return this.skipTakeOptions;
		}

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			queryable = this.PrimaryExpression.ApplyTo(queryable);
			foreach(ISortExpression<T> secondaryExpression in this.secondaryExpressions)
			{
				IOrderedQueryable<T> orderedQueryable = (IOrderedQueryable<T>)queryable;
				queryable = secondaryExpression.ApplyTo(orderedQueryable);
			}

			if(this.pagingOptions != null)
			{
				queryable = this.pagingOptions.ApplyTo(queryable);
			}

			if(this.skipTakeOptions != null)
			{
				queryable = this.skipTakeOptions.ApplyTo(queryable);
			}

			return queryable;
		}

		/// <inheritdoc />
		public bool TryGetPagingOptions(out IPagingOptions<T>? pagingOptions)
		{
			pagingOptions = this.pagingOptions;
			return this.pagingOptions != null;
		}

		/// <inheritdoc />
		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions)
		{
			skipTakeOptions = this.skipTakeOptions;
			return this.skipTakeOptions != null;
		}

		/// <inheritdoc />
		public bool TryGetSortingOptions(out ISortingOptions<T>? sortingOptions)
		{
			sortingOptions = this;
			return true;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			string orderByString = this.PrimaryExpression.ToString();
			string thenByString = this.secondaryExpressions.Select(x => x.ToString()).Aggregate((s1, s2) => string.Concat(s1, ", ", s2));
			string sortingOptionsString = "(OrderBy: {0}, ThenBy: {1})".FormatInvariantWith(orderByString, thenByString);

			string pagingOptionsString = this.pagingOptions != null ? this.pagingOptions.ToString() : "none";
			string skipTakeOptionsString = this.skipTakeOptions != null ? this.skipTakeOptions.ToString() : "none";

			return "QueryOptions<{0}>(Sorting: {1}, Paging: {2}, SkipTake: {3})"
				.FormatInvariantWith(typeof(T).Name, sortingOptionsString, pagingOptionsString, skipTakeOptionsString);
		}
	}
}
