namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;

	internal sealed class QueryOptionsImpl<T> : IQueryOptions<T> where T : class
	{
		private ISortingOptions<T> SortingOptions { get; set; }

		internal ISkipTakeOptions<T> SkipTakeOptions { get; set; }

		internal IPagingOptions<T> PagingOptions { get; set; }

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			queryable = this.SortingOptions?.ApplyTo(queryable);
			queryable = this.SkipTakeOptions?.ApplyTo(queryable);
			queryable = this.PagingOptions?.ApplyTo(queryable);

			return queryable;
		}

		/// <inheritdoc />
		public bool IsEmpty()
		{
			return false;
		}

		/// <inheritdoc />
		public bool TryGetPagingOptions(out IPagingOptions<T> options)
		{
			options = this.PagingOptions;
			return this.PagingOptions is not null;
		}

		/// <inheritdoc />
		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T> options)
		{
			options = this.SkipTakeOptions;
			return this.SkipTakeOptions is not null;
		}

		/// <inheritdoc />
		public bool TryGetSortingOptions(out ISortingOptions<T> options)
		{
			options = this.SortingOptions;
			return this.SortingOptions is not null;
		}

		internal ISortingOptions<T> OrderBy(Expression<Func<T, object>> sortExpression)
		{
			this.SortingOptions ??= new SortingOptions<T>(this, sortExpression);
			return this.SortingOptions;
		}

		internal ISortingOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			this.SortingOptions ??= new SortingOptions<T>(this, sortExpression, true);
			return this.SortingOptions;
		}

		internal ISkipTakeOptions<T> Skip(int skipAmount)
		{
			this.SkipTakeOptions ??= new SkipTakeOptions<T>(this, skipAmount);
			return this.SkipTakeOptions;
		}

		internal ISkipTakeOptions<T> Take(int takeAmount)
		{
			this.SkipTakeOptions ??= new SkipTakeOptions<T>(this, take: takeAmount);
			return this.SkipTakeOptions;
		}

		internal ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount)
		{
			this.SkipTakeOptions ??= new SkipTakeOptions<T>(this, skipAmount, takeAmount);
			return this.SkipTakeOptions;
		}

		internal IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.PagingOptions ??= new PagingOptions<T>(this, pageNumber, pageSize);
			return this.PagingOptions;
		}

		internal IPagingOptions<T> Paging()
		{
			this.PagingOptions ??= new PagingOptions<T>(this);
			return this.PagingOptions;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			if(this.SortingOptions is not null)
			{
				return this.SortingOptions.ToString();
			}

			const string sortingOptionsString = "none";
			string pagingOptionsString = this.PagingOptions != null ? this.PagingOptions.ToString() : "none";
			string skipTakeOptionsString = this.SkipTakeOptions != null ? this.SkipTakeOptions.ToString() : "none";

			return "QueryOptions<{0}>(Sorting: {1}, Paging: {2}, SkipTake: {3})"
				.FormatInvariantWith(typeof(T).Name, sortingOptionsString, pagingOptionsString, skipTakeOptionsString);
		}
	}
}
