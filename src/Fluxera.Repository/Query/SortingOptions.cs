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

		public SortingOptions(Expression<Func<T, object>> sortExpression, bool isDescending = false)
		{
			this.PrimaryExpression = new SortExpression<T>(sortExpression, isDescending);
		}

		private IPagingOptions<T>? PagingOptions { get; set; }

		private ISkipTakeOptions<T>? TakeOptions { get; set; }

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
			this.PagingOptions = new PagingOptions<T>(pageNumber, pageSize);
			return this.PagingOptions;
		}

		/// <inheritdoc />
		public IPagingOptions<T> Paging()
		{
			this.PagingOptions = new PagingOptions<T>();
			return this.PagingOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Skip(int skip)
		{
			this.TakeOptions = new SkipTakeOptions<T>(skip);
			return this.TakeOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Take(int take)
		{
			this.TakeOptions = new SkipTakeOptions<T>(take: take);
			return this.TakeOptions;
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

			if(this.PagingOptions != null)
			{
				queryable = this.PagingOptions.ApplyTo(queryable);
			}

			if(this.TakeOptions != null)
			{
				queryable = this.TakeOptions.ApplyTo(queryable);
			}

			return queryable;
		}

		/// <inheritdoc />
		public bool TryGetPagingOptions(out IPagingOptions<T>? pagingOptions)
		{
			pagingOptions = this.PagingOptions;
			return this.PagingOptions != null;
		}

		/// <inheritdoc />
		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions)
		{
			skipTakeOptions = this.TakeOptions;
			return this.TakeOptions != null;
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

			string pagingOptionsString = this.PagingOptions != null ? this.PagingOptions.ToString() : "none";
			string skipTakeOptionsString = this.TakeOptions != null ? this.TakeOptions.ToString() : "none";

			return "QueryOptions<{0}>(Sorting: {1}, Paging: {2}, SkipTake: {3})"
				.FormatInvariantWith(typeof(T).Name, sortingOptionsString, pagingOptionsString, skipTakeOptionsString);
		}
	}
}
