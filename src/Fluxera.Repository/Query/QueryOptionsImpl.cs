namespace Fluxera.Repository.Query
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;

	internal sealed class QueryOptionsImpl<T> : IQueryOptions<T>, IIncludeOptions<T>, ISortingOptions<T>, ISkipTakeOptions<T>, IPagingOptions<T>
		where T : class
	{
		private readonly IIncludeApplier includeApplier;

		public QueryOptionsImpl(IIncludeApplier includeApplier = null)
		{
			this.includeApplier = includeApplier;
		}

		internal IIncludeOptions<T> IncludeOptions { get; set; }

		internal ISortingOptions<T> SortingOptions { get; set; }

		internal ISkipTakeOptions<T> SkipTakeOptions { get; set; }

		internal IPagingOptions<T> PagingOptions { get; set; }

		/// <inheritdoc />
		IQueryable<T> IQueryOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			if(this.IncludeOptions is not null)
			{
				queryable = this.IncludeOptions.ApplyTo(queryable);
			}

			if(this.SortingOptions is not null)
			{
				queryable = this.SortingOptions.ApplyTo(queryable);
			}

			if(this.SkipTakeOptions is not null)
			{
				queryable = this.SkipTakeOptions.ApplyTo(queryable);
			}

			if(this.PagingOptions is not null)
			{
				queryable = this.PagingOptions.ApplyTo(queryable);
			}

			return queryable;
		}

		/// <inheritdoc />
		bool IQueryOptions<T>.IsEmpty()
		{
			return false;
		}

		internal IIncludeOptions<T> Include(Expression<Func<T, object>> includeExpression)
		{
			this.IncludeOptions ??= new IncludeOptions<T>(this, includeExpression, this.includeApplier);
			return this.IncludeOptions;
		}

		internal ISortingOptions<T> OrderBy(Expression<Func<T, object>> sortExpression)
		{
			this.SortingOptions ??= new SortingOptions<T>(this, sortExpression, false);
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

		/// <inheritdoc />
		IIncludeOptions<T> IIncludeOptions<T>.Include(Expression<Func<T, object>> includeExpression)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISortingOptions<T> IIncludeOptions<T>.OrderBy(Expression<Func<T, object>> sortExpression)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISortingOptions<T> IIncludeOptions<T>.OrderByDescending(Expression<Func<T, object>> sortExpression)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISortingOptions<T> ISortingOptions<T>.ThenBy(Expression<Func<T, object>> sortExpression)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISortingOptions<T> ISortingOptions<T>.ThenByDescending(Expression<Func<T, object>> sortExpression)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISortingOptions<T>.Skip(int skip)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISkipTakeOptions<T>.Take(int takeAmount)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISkipTakeOptions<T>.Skip(int skipAmount)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISortingOptions<T>.Take(int take)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISortingOptions<T>.SkipTake(int skip, int take)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IPagingOptions<T> ISortingOptions<T>.Paging(int pageNumber, int pageSize)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IPagingOptions<T> ISortingOptions<T>.Paging()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> IIncludeOptions<T>.Skip(int skip)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> IIncludeOptions<T>.Take(int take)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> IIncludeOptions<T>.SkipTake(int skip, int take)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IPagingOptions<T> IIncludeOptions<T>.Paging(int pageNumber, int pageSize)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IPagingOptions<T> IIncludeOptions<T>.Paging()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IPagingOptions<T> IPagingOptions<T>.PageNumber(int pageNumberAmount)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IPagingOptions<T> IPagingOptions<T>.PageSize(int pageSizeAmount)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryOptions<T> IIncludeOptions<T>.Build()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryOptions<T> ISortingOptions<T>.Build()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryOptions<T> ISkipTakeOptions<T>.Build()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryOptions<T> IPagingOptions<T>.Build()
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryable<T> IIncludeOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryable<T> ISortingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryable<T> ISkipTakeOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new UnreachableException();
		}

		/// <inheritdoc />
		IQueryable<T> IPagingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new UnreachableException();
		}
	}
}
